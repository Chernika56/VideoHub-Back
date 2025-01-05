using BackEnd.DataBase.Context;
using BackEnd.Users.Endpoints;
using BackEnd.Users.Services;
using BackEnd.Utils.Policies;
using BackEnd.Utils.Roles;
using BackEnd.Video.Endpoints;
using BackEnd.Video.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appparams.json");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp", builder =>
        builder.WithOrigins("http://localhost:5173")
            .AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var mySqlVersion = builder.Configuration.GetValue<string>("MySQLVersion");
builder.Services.AddDbContext<VideoHubDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), ServerVersion.Parse(mySqlVersion)));

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

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<VideoService>();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(PolicyType.AdministratorPolicy, policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, RoleType.Administrator);
    }
    );
    options.AddPolicy(PolicyType.RegularUserPolicy, policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, RoleType.AllRoles);
    }
    );
});

var app = builder.Build();

app.UseCors("AllowVueApp");
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
globalGroup.MapGroup(@"/user").MapUser();
globalGroup.MapGroup(@"/video").MapVideo();

app.UseHttpsRedirection();

app.Run();
