using BackEnd.Authorization.Endpoints;
using BackEnd.Authorization.Services;
using BackEnd.Cameras.Endpoints;
using BackEnd.Cameras.Services;
using BackEnd.DB.Context;
using BackEnd.Folders.Endpoints;
using BackEnd.Folders.Services;
using BackEnd.Messages.Endpoints;
using BackEnd.Messages.Services;
using BackEnd.Mosaics.Endpoints;
using BackEnd.Mosaics.Services;
using BackEnd.Organizations.Enpoints;
using BackEnd.Organizations.Services;
using BackEnd.Presets.Endpoints;
using BackEnd.Presets.Services;
using BackEnd.ServerServices;
using BackEnd.Streamers.Endpoints;
using BackEnd.Users.Endpoints;
using BackEnd.Users.Services;
using BackEnd.Utils.Policies;
using BackEnd.Utils.Roles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appparams.json");
builder.Configuration.AddEnvironmentVariables();
//builder.Configuration.AddJsonFile("appsettings.json");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNuxt", builder =>
        builder.WithOrigins("http://localhost:3000", "http://172.16.0.48:3000", "http://10.30.255.23:3000")
            .AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient<IStreamerService, BackEnd.ServerServices.StreamerService>(client =>
{
    string username = builder.Configuration["Streamer:Login"]!;
    string password = builder.Configuration["Streamer:Password"]!;

    var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "VideoHub", Version = "v1" });
    //c.OperationFilter<FileUploadOperation>();
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Введите токен JWT с префиксом Bearer",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var config = builder.Configuration;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidAudience = config["JwtParameters:Audience"],
            ValidateAudience = true,
            ValidIssuer = config["JwtParameters:Issuer"],
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtParameters:Key"]!)),
            ClockSkew = TimeSpan.Zero
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.ContainsKey("jwtToken"))
                {
                    context.Token = context.Request.Cookies["jwtToken"];
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(PolicyType.AdministratorPolicy, policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, RoleType.Administrator);
    }
    );
    options.AddPolicy(PolicyType.UserPolicy, policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, RoleType.AllRoles);
    }
    );
    options.AddPolicy(PolicyType.OrganizationAdminPolicy, policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, RoleType.Administrators);
    }
    );
});

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ProfileService>();
builder.Services.AddScoped<CameraService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuthHelper>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<MosaicsService>();
builder.Services.AddScoped<PresetService>();
builder.Services.AddScoped<OrganizationService>();
builder.Services.AddScoped<OrganizationUsersService>();
builder.Services.AddScoped<OrganizationFoldersService>();
builder.Services.AddScoped<OrganizationFolderUserService>();
builder.Services.AddScoped<FolderService>();
builder.Services.AddScoped<MessageService>();
builder.Services.AddScoped<BackEnd.Streamers.Services.StreamerService>();

var app = builder.Build();

app.UseCors("AllowNuxt");
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

var globalGroup = app.MapGroup("/api/v1.0");
globalGroup.MapGroup(@"/users").MapUsers();
globalGroup.MapGroup(@"/profile").MapProfile();
globalGroup.MapGroup(@"/cameras").MapCameras();
globalGroup.MapGroup(@"/auth").MapAuth();
globalGroup.MapGroup(@"/mosaics").MapMosaics();
globalGroup.MapGroup(@"/presets").MapPresets();
globalGroup.MapGroup(@"/folders").MapFolders();
globalGroup.MapGroup(@"/messages").MapMessages();
globalGroup.MapGroup(@"/streamers").MapStreamers();

var organizationsGroup = globalGroup.MapGroup("/organizations");
organizationsGroup.MapOrganizations();
organizationsGroup.MapOrganizationUsers();
organizationsGroup.MapOrganizationFolders();
organizationsGroup.MapOrganizationFolderUsers();

//app.UseHttpsRedirection();

app.Run();


// сначала фронт получает от бэка токен(playback_config.token), этот токен свой для каждого аккаунта, то есть хранится в БД
// этот токен добавляется в конец ссылки на поток `${this.host}/${s.name}/preview.jpg?token=${token}`
// видео плейер идет по этой ссылке, с токеном на сервер Flussonic, он видя токен проверяет, открыта ли сессия с помощью session_id
// session_id = hash(name(потока) + ip(клиента) + token). Flussonic хранит???
// если session_id нету, то отправляет запрос /play на мой сервер, а мой сервер должен дать 200(ОК)


// мне на сервере нужно сделать: генерацию playback_config.token для каждого юзера
// обработку запроса /play - session_id наверное сохранять в БД, возвращать 200, если token совпал, можно посчитать сколько потоков юзер смотрит

// не просто /folders ,а /organizations/id/folders

// session - сессионный ключ, отправляется с каждым запросом, apiKey - ключ для авторизации в vsaas.io ...

