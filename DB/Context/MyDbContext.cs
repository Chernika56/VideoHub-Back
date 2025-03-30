using BackEnd.DB.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

//{
//    "comment": "comment1",
//  "coordinates": "57 55",
//  "dvrDepth": 1,
//  "dvrLockDays": 1,
//  "dvrPath": "dvr",
//  "dvrSpace": 1,
//  "folderId": 1,
//  "motionDetectorEnabled": true,
//  "onvifProfile": "media_profile1",
//  "onvifPTZ": true,
//  "onvifURL": "http://admin:321678!Qw@172.16.0.239",
//  "permissions": {
//        "view": true,
//    "edit": true,
//    "ptz": true,
//    "dvr": true,
//    "dvrDepthLimit": 1,
//    "actions": true
//  },
//  "postalAddress": "ул. Калатушкина",
//  "presetId": 1,
//  "streamUrl": "rtsp://admin:321678!Qw@172.16.0.239:554/media/video1",
//  "streamerId": 1,
//  "subStreamUrl": "",
//  "title": "Cam_ATS-327"
//}

namespace BackEnd.DB.Context
{
    public class MyDbContext : DbContext
    {
        public MyDbContext()
        {
            Database.EnsureCreated();
        }

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<UsersEntity> Users { get; set; } = null!;

        public DbSet<OrganizationsEntity> Organizations { get; set; } = null!;

        public DbSet<M2mUsersOrganizationsEntity> M2mUsersOrganizations { get; set; } = null!;

        public DbSet<FoldersEntity> Folders { get; set; } = null!;

        public DbSet<M2mUsersFoldersEntity> M2mUsersFolders { get; set; } = null!;

        public DbSet<CamerasEntity> Cameras { get; set; } = null!;

        public DbSet<PresetsEntity> Presets { get; set; } = null!;

        public DbSet<EventsEntity> Events { get; set; } = null!;

        public DbSet<StreamersEntity> Streamers { get; set; } = null!;

        public DbSet<MosaicsEntity> Mosaics { get; set; } = null!;

        public DbSet<M2mMosaicsCamerasEntity> M2mMosaicsCameras { get; set; } = null!;

        public DbSet<MessagesEntity> Messages { get; set; } = null!;

        public DbSet<M2mUsersCamerasEntity> m2MUsersCameras { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsersEntity>(entity =>
            {
                // Table Name
                entity.ToTable("Users");

                // Primary Key
                entity.HasKey(u => u.Id);

                // Alternate Key, Indexes
                entity.HasAlternateKey(u => u.Login);

                // Other columns: Name, limit, defaultValue, autoIncriment
                entity.Property(u => u.Id)
                    .HasColumnName("usr_id")
                    .ValueGeneratedOnAdd();

                entity.Property(u => u.Login)
                    .HasColumnName("usr_login")
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(u => u.Password)
                    .HasColumnName("usr_password")
                    .IsRequired();

                entity.Property(u => u.Name)
                    .HasColumnName("usr_name")
                    .HasMaxLength(50);

                entity.Property(u => u.Email)
                    .HasColumnName("usr_email")
                    .HasMaxLength(100);

                entity.Property(u => u.Phone)
                    .HasColumnName("usr_phone")
                    .HasMaxLength(15);

                entity.Property(u => u.Note)
                    .HasColumnName("usr_note")
                    .HasMaxLength(1000);

                entity.Property(u => u.AccessLevel)
                    .HasColumnName("usr_accessLevel")
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(u => u.MaxSessions)
                    .HasColumnName("usr_maxSessions");

                entity.Property(u => u.Disabled)
                    .HasColumnName("usr_disabled")
                    .HasDefaultValue(false);

                entity.Property(u => u.IsLoggedIn)
                    .HasColumnName("usr_isLoggedIn")
                    .HasDefaultValue(false);

                entity.Property(u => u.Token)
                    .HasColumnName("usr_token")
                    .HasMaxLength(50)
                    .IsRequired();

                // Foreign Key


                // Default Data
                entity.HasData(new UsersEntity
                {
                    Id = 1,
                    Login = "Admin",
                    Password = CreateHashCode("admin"),
                    Name = "Admin",
                    AccessLevel = "Admin",
                    Token = "thisIsPersonalUserTokenAdmin",
                    MaxSessions = 100
                });
            });

            modelBuilder.Entity<OrganizationsEntity>(entity =>
            {
                // Table Name
                entity.ToTable("Organizations");

                // Primary Key
                entity.HasKey(o => o.Id);

                // Alternate Key, Indexes
                entity.HasIndex(o => o.Title).IsUnique();

                // Other columns: Name, limit, defaultValue, autoIncriment
                entity.Property(o => o.Id)
                    .HasColumnName("org_id")
                    .ValueGeneratedOnAdd();

                entity.Property(o => o.Title)
                    .HasColumnName("org_title")
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(o => o.UserCount)
                    .HasColumnName("org_userCount")
                    .HasDefaultValue(0)
                    .IsRequired();

                entity.Property(o => o.CameraCount)
                    .HasColumnName("org_cameraCount")
                    .HasDefaultValue(0)
                    .IsRequired();

                entity.Property(o => o.MosaicCount)
                    .HasColumnName("org_mosaicCount")
                    .HasDefaultValue(0)
                    .IsRequired();

                entity.Property(o => o.UserLimit)
                    .HasColumnName("org_userLimit");

                entity.Property(o => o.CameraLimit)
                    .HasColumnName("org_cameraLimit");

                entity.Property(o => o.IsDefault)
                    .HasColumnName("org_isDefault")
                    .HasDefaultValue(false)
                    .IsRequired();

                entity.Property(o => o.OwnerId)
                    .HasColumnName("org_ownerId")
                    .IsRequired();

                // Foreign Key


                // Default Data
                entity.HasData(new OrganizationsEntity
                {
                    Id = 1,
                    Title = "Default",
                    UserCount = 1,
                    IsDefault = true,
                    OwnerId = 1,
                    CameraCount = 0
                });
            });

            modelBuilder.Entity<M2mUsersOrganizationsEntity>(entity =>
            {
                // Table Name
                entity.ToTable("m2m_Users_Organizations");

                // Primary Key
                entity.HasKey(e => new { e.UserId, e.OrganizationId });

                // Alternate Key, Indexes

                // Other columns: Name, limit, defaultValue, autoIncriment
                entity.Property(e => e.UserId)
                    .HasColumnName("uo_usrId");

                entity.Property(e => e.OrganizationId)
                    .HasColumnName("uo_orgId");

                entity.Property(e => e.IsMember)
                    .HasColumnName("uo_isMember")
                    .HasDefaultValue(false)
                    .IsRequired();

                entity.Property(e => e.IsAdmin)
                    .HasColumnName("uo_isAdmin")
                    .HasDefaultValue(false)
                    .IsRequired();

                // Foreign Key
                entity.HasOne(e => e.User).WithMany(u => u.M2mUsersOrganizations)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                //.HasConstraintName("fk_m2m_Users_Organizations_usr");

                entity.HasOne(e => e.Organization).WithMany(o => o.M2mUsersOrganizations)
                    .HasForeignKey(e => e.OrganizationId)
                    .OnDelete(DeleteBehavior.Cascade);
                //.HasConstraintName("fk_m2m_Users_Organizations_org");

                // Default Data
                entity.HasData(new M2mUsersOrganizationsEntity
                {
                    UserId = 1,
                    OrganizationId = 1,
                    IsAdmin = true,
                    IsMember = true
                });
            });

            modelBuilder.Entity<FoldersEntity>(entity =>
            {
                // Table Name
                entity.ToTable("Folders");

                // Primary Key
                entity.HasKey(f => f.Id);

                // Alternate Key, Indexes

                // Other columns: Name, limit, defaultValue, autoIncriment
                entity.Property(f => f.Id)
                    .HasColumnName("fld_id")
                    .ValueGeneratedOnAdd();

                entity.Property(f => f.OrganizationId)
                    .HasColumnName("fld_orgId")
                    .IsRequired();

                entity.Property(f => f.ParentId)
                    .HasColumnName("fld_parentId");

                entity.Property(f => f.Title)
                    .HasColumnName("fld_Title")
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(f => f.CameraCount)
                    .HasColumnName("fld_cameraCount")
                    .HasDefaultValue(0)
                    .IsRequired();

                entity.Property(f => f.HierarchyLevel)
                    .HasColumnName("fld_hierarchyLevel")
                    .HasDefaultValue(0)
                    .IsRequired();

                entity.Property(f => f.CoordinatesLatitude)
                    .HasColumnName("fld_coordinatesLatitude");

                entity.Property(f => f.CoordinatesLongitude)
                    .HasColumnName("fld_coordinatesLongitude");

                // Foreign Key
                entity.HasOne(f => f.Organization).WithMany(o => o.Folders)
                    .HasForeignKey(f => f.OrganizationId)
                    .OnDelete(DeleteBehavior.Cascade);
                //.HasConstraintName("fk_Folders_org");

                entity.HasOne(f => f.ParentFolder).WithMany(f => f.ChildFolders)
                    .HasForeignKey(f => f.ParentId)
                    .OnDelete(DeleteBehavior.Cascade);
                //.HasConstraintName("fk_Folders_parent");

                // Default Data
                entity.HasData(new FoldersEntity
                {
                    Id = 1,
                    Title = "Default",
                    OrganizationId = 1,
                    HierarchyLevel = 0,
                    CameraCount = 0,
                });
            });

            modelBuilder.Entity<M2mUsersFoldersEntity>(entity =>
            {
                // Table Name
                entity.ToTable("m2m_Users_Folders");

                // Primary Key
                entity.HasKey(e => new { e.UserId, e.FolderId });

                // Alternate Key, Indexes

                // Other columns: Name, limit, defaultValue, autoIncriment
                entity.Property(e => e.UserId)
                    .HasColumnName("uf_usrId");

                entity.Property(e => e.FolderId)
                    .HasColumnName("uf_fldId");

                entity.Property(e => e.CanView)
                    .HasColumnName("uo_canView")
                    .HasDefaultValue(false)
                    .IsRequired();

                // Foreign Key
                entity.HasOne(e => e.User).WithMany(u => u.M2mUsersFolders)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                //.HasConstraintName("fk_m2m_Users_Folders_usr");

                entity.HasOne(e => e.Folder).WithMany(o => o.M2mUsersFolders)
                    .HasForeignKey(e => e.FolderId)
                    .OnDelete(DeleteBehavior.Cascade);
                //.HasConstraintName("fk_m2m_Users_Folder_fld");

                // Default Data
                entity.HasData(new M2mUsersFoldersEntity
                {
                    UserId = 1,
                    FolderId = 1,
                    CanView = true
                });
            });

            modelBuilder.Entity<CamerasEntity>(entity =>
            {
                // Table Name
                entity.ToTable("Cameras");

                // Primary Key
                entity.HasKey(c => c.Name);

                // Alternate Key, Indexes
                entity.HasAlternateKey(c => c.Name);

                // Other columns: Name, limit, defaultValue, autoIncriment
                entity.Property(c => c.Name)
                    .HasColumnName("cam_name")
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(c => c.FolderId)
                    .HasColumnName("cam_fldId")
                    .IsRequired();

                entity.Property(c => c.PresetId)
                    .HasColumnName("cam_prstId")
                    .IsRequired();

                entity.Property(c => c.StreamerId)
                    .HasColumnName("cam_strmId")
                    .IsRequired();

                entity.Property(c => c.OrganizationId)
                    .HasColumnName("cam_orgId")
                    .IsRequired();

                entity.Property(c => c.Comment)
                    .HasColumnName("cam_comment")
                    .HasMaxLength(100);

                entity.Property(c => c.Coordinates)
                    .HasColumnName("cam_coordinates")
                    .HasMaxLength(100);

                entity.Property(c => c.Enabled)
                    .HasColumnName("cam_enabled")
                    .HasDefaultValue(false)
                    .IsRequired();

                entity.Property(c => c.DVRDepth)
                    .HasColumnName("cam_dvrDepth");

                entity.Property(c => c.DVRLockDays)
                    .HasColumnName("cam_dvrLockDays");

                entity.Property(c => c.DVRPath)
                    .HasColumnName("cam_dvrPath")
                    .HasMaxLength(10);

                entity.Property(c => c.DVRSpace)
                    .HasColumnName("cam_dvrSpace");

                entity.Property(c => c.LastEventTime)
                    .HasColumnName("cam_lastEventTime")
                    .HasDefaultValue(DateTimeOffset.Now)
                    .IsRequired();

                entity.Property(c => c.MotionDetectorEnabled)
                    .HasColumnName("cam_motionDetectorEnabled");

                entity.Property(c => c.OnvifProfile)
                    .HasColumnName("cam_onvifProfile")
                    .HasMaxLength(100);

                entity.Property(c => c.OnvifPTZ)
                    .HasColumnName("cam_onvifPtz");

                entity.Property(c => c.OnvifURL)
                    .HasColumnName("cam_onvifUrl")
                    .HasMaxLength(100);

                entity.Property(c => c.View)
                    .HasColumnName("cam_view");

                entity.Property(c => c.Edit)
                    .HasColumnName("cam_edit");

                entity.Property(c => c.PTZ)
                    .HasColumnName("cam_ptz");

                entity.Property(c => c.DVR)
                    .HasColumnName("cam_dvr");

                entity.Property(c => c.DVRDepthLimit)
                    .HasColumnName("cam_dvrDepthLimit");

                entity.Property(c => c.Actions)
                    .HasColumnName("cam_actions");

                entity.Property(c => c.PostalAddress)
                    .HasColumnName("cam_postalAddress");

                entity.Property(c => c.StreamUrl)
                    .HasColumnName("cam_streamUrl")
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(c => c.SubStreamUrl)
                    .HasColumnName("cam_subStreamUrl")
                    .HasMaxLength(100);

                entity.Property(c => c.Title)
                    .HasColumnName("cam_title")
                    .HasMaxLength(100)
                    .IsRequired();

                // Foreign Key
                entity.HasOne(c => c.Folder).WithMany(f => f.Cameras)
                    .HasForeignKey(c => c.FolderId)
                    .OnDelete(DeleteBehavior.Cascade);
                //.HasConstraintName("fk_Cameras_fld");

                entity.HasOne(c => c.Preset).WithMany(p => p.Cameras)
                    .HasForeignKey(c => c.PresetId)
                    .OnDelete(DeleteBehavior.Restrict);
                //.HasConstraintName("fk_Cameras_prst");

                entity.HasOne(c => c.Streamer).WithMany(s => s.Cameras)
                    .HasForeignKey(c => c.StreamerId)
                    .OnDelete(DeleteBehavior.Restrict);
                //.HasConstraintName("fk_Cameras_strm");

                // Default Data
                //entity.HasData(new CamerasEntity
                //{
                //    Name = "Cam_ATS-327",
                //    Comment = "comment1",
                //    Coordinates = "57 55",
                //    DVRDepth = 1,
                //    DVRLockDays = 1,
                //    DVRPath = "dvr",
                //    DVRSpace = 1,
                //    Enabled = true,
                //    FolderId = 1,
                //    LastEventTime = DateTimeOffset.Now,
                //    MotionDetectorEnabled = true,
                //    OnvifProfile = "media_profile1",
                //    OnvifPTZ = true,
                //    OnvifURL = "http://admin:321678!Qw@172.16.0.239",
                //    OrganizationId = 1,
                //    View = true,
                //    Edit = true,
                //    PTZ = true,
                //    DVR = true,
                //    DVRDepthLimit = 1,
                //    Actions = true,
                //    PostalAddress = "ул. Калатушкина",
                //    PresetId = 1,
                //    StreamUrl = "rtsp://admin:321678!Qw@172.16.0.239:554/media/video1",
                //    StreamerId = 1,
                //    SubStreamUrl = "",
                //    Title = "Cam_ATS-327",
                //});
            });

            modelBuilder.Entity<PresetsEntity>(entity =>
            {
                // Table Name
                entity.ToTable("Presets");

                // Primary Key
                entity.HasKey(p => p.Id);

                // Alternate Key, Indexes


                // Other columns: Name, limit, defaultValue, autoIncriment
                entity.Property(p => p.Id)
                      .HasColumnName("prst_id")
                      .ValueGeneratedOnAdd();

                entity.Property(p => p.Title)
                      .HasColumnName("prst_title")
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(p => p.DVRDepth)
                    .HasColumnName("prst_dvrDepth")
                    .IsRequired();

                entity.Property(p => p.DVRLockDays)
                    .HasColumnName("prst_dvrLockDays")
                    .IsRequired();

                entity.Property(p => p.DVRSpace)
                    .HasColumnName("prst_dvrSpace");

                entity.Property(p => p.IsAdjustable)
                    .HasColumnName("prst_isAdjustable")
                    .HasDefaultValue(true)
                    .IsRequired();

                entity.Property(p => p.IsDefault)
                    .HasColumnName("prst_isDefault")
                    .HasDefaultValue(false)
                    .IsRequired();

                entity.Property(p => p.PreciseTrumbnailsDays)
                    .HasColumnName("prst_preciseTrumbnailsDays")
                    .HasDefaultValue(0)
                    .IsRequired();

                entity.Property(p => p.IsDeleted)
                    .HasColumnName("prst_isDeleted")
                    .HasDefaultValue(false)
                    .IsRequired();

                // Foreign Key

                // Default Data
                entity.HasData(new PresetsEntity
                {
                    Id = 1,
                    Title = "preset1",
                    DVRDepth = 1,
                    DVRLockDays = 1,
                    DVRSpace = 1,
                    IsAdjustable = true,
                    IsDefault = true,
                    PreciseTrumbnailsDays = 1,
                    IsDeleted = false
                });
            });

            modelBuilder.Entity<EventsEntity>(entity =>
            {
                // Table Name
                entity.ToTable("Enents");

                // Primary Key
                entity.HasKey(e => e.Id);

                // Alternate Key, Indexes

                // Other columns: Name, limit, defaultValue, autoIncriment
                entity.Property(e => e.Id)
                    .HasColumnName("envt_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CameraName)
                    .HasColumnName("envt_camName")
                    .IsRequired();

                entity.Property(e => e.Source)
                    .HasColumnName("envt_source")
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.Type)
                    .HasColumnName("envt_type")
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.HasPreview)
                    .HasColumnName("envt_hasPreview")
                    .HasDefaultValue(false)
                    .IsRequired();

                entity.Property(e => e.StartAt)
                    .HasColumnName("envt_startAt")
                    .IsRequired();

                entity.Property(e => e.EndAt)
                    .HasColumnName("envt_endAt")
                    .IsRequired();

                // Foreign Key
                entity.HasOne(e => e.Camera).WithMany(c => c.Events)
                    .HasForeignKey(e => e.CameraName)
                    .OnDelete(DeleteBehavior.Cascade);
                //.HasConstraintName("fk_Events_cam");

            });

            modelBuilder.Entity<StreamersEntity>(entity =>
            {
                // Table Name
                entity.ToTable("Streamers");

                // Primary Key
                entity.HasKey(s => s.Id);

                // Alternate Key, Indexes


                // Other columns: Name, limit, defaultValue, autoIncriment
                entity.Property(s => s.Id)
                      .HasColumnName("strm_id")
                      .ValueGeneratedOnAdd();

                entity.Property(s => s.HostName)
                      .HasColumnName("strm_hostName")
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(s => s.ApiUrl)
                      .HasColumnName("strm_apiUrl")
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(s => s.Port)
                      .HasColumnName("strm_port")
                      .IsRequired();

                entity.Property(s => s.IsLocal)
                      .HasColumnName("strm_isLocal")
                      .IsRequired();

                entity.Property(s => s.DVRPath)
                      .HasColumnName("strm_dvrPath")
                      .HasMaxLength(100)
                      .IsRequired();

                // Foreign Key

                // Default Data
                entity.HasData(new StreamersEntity
                {
                    Id = 1,
                    HostName = "flussonic",
                    ApiUrl = "https://172.16.0.48",
                    Port = 80,
                    IsLocal = false,
                    DVRPath = "dvr"
                });
            });

            modelBuilder.Entity<MosaicsEntity>(entity =>
            {
                // Table Name
                entity.ToTable("Mosaics");

                // Primary Key
                entity.HasKey(m => m.Id);

                // Alternate Key, Indexes

                // Other columns: Name, limit, defaultValue, autoIncriment
                entity.Property(m => m.Id)
                      .HasColumnName("mosc_id")
                      .ValueGeneratedOnAdd();

                entity.Property(m => m.OrganizationId)
                      .HasColumnName("mosc_orgId")
                      .IsRequired();

                entity.Property(m => m.Title)
                      .HasColumnName("mosc_title")
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(m => m.Type)
                      .HasColumnName("mosc_type")
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(m => m.Visible)
                      .HasColumnName("mosc_visible")
                      .HasDefaultValue(true)
                      .IsRequired();

                // Foreign Key
                entity.HasOne(m => m.Organization).WithMany(o => o.Mosaics)
                    .HasForeignKey(m => m.OrganizationId)
                    .OnDelete(DeleteBehavior.Cascade);
                //.HasConstraintName("fk_Mosaics_org");
            });

            modelBuilder.Entity<M2mMosaicsCamerasEntity>(entity =>
            {
                // Table Name
                entity.ToTable("m2m_Mosaics_Cameras");

                // Primary Key
                entity.HasKey(e => new { e.MosaicId, e.CameraName });

                // Alternate Key, Indexes

                // Other columns: Name, limit, defaultValue, autoIncriment
                entity.Property(e => e.MosaicId)
                    .HasColumnName("mc_moscId");

                entity.Property(e => e.CameraName)
                    .HasColumnName("mc_camName");

                // Foreign Key
                entity.HasOne(e => e.Mosaic).WithMany(u => u.M2mMosaicsCameras)
                    .HasForeignKey(e => e.MosaicId)
                    .OnDelete(DeleteBehavior.Cascade);
                //.HasConstraintName("fk_m2m_Mosaics_Cameras_mosc");
                entity.HasOne(e => e.Camera).WithMany(o => o.M2mMosaicsCameras)
                    .HasForeignKey(e => e.CameraName)
                    .OnDelete(DeleteBehavior.Cascade);
                //.HasConstraintName("fk_m2m_Mosaics_Cameras_cam");
            });

            modelBuilder.Entity<MessagesEntity>(entity =>
            {
                // Table Name
                entity.ToTable("Messages");

                // Primary Key
                entity.HasKey(m => m.Id);

                // Alternate Key, Indexes


                // Other columns: Name, limit, defaultValue, autoIncriment
                entity.Property(m => m.Id)
                    .HasColumnName("mess_id")
                    .ValueGeneratedOnAdd();

                entity.Property(m => m.UserId)
                    .HasColumnName("mess_userId")
                    .IsRequired();

                entity.Property(m => m.SenderId)
                    .HasColumnName("mess_senderId")
                    .IsRequired();

                entity.Property(m => m.Title)
                    .HasColumnName("mess_title")
                    .HasMaxLength(50);

                entity.Property(m => m.Body)
                    .HasColumnName("mess_body")
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(m => m.Type)
                    .HasColumnName("mess_type")
                    .HasMaxLength(20);

                entity.Property(m => m.IsPush)
                    .HasColumnName("mess_isPush")
                    .HasDefaultValue(false)
                    .IsRequired();

                entity.Property(m => m.IsDashboard)
                    .HasColumnName("mess_isDashboard")
                    .HasDefaultValue(false)
                    .IsRequired();

                entity.Property(m => m.IsDeleted)
                    .HasColumnName("mess_isDeleted")
                    .HasDefaultValue(false)
                    .IsRequired();

                entity.Property(m => m.WasRead)
                    .HasColumnName("mess_wasRead")
                    .HasDefaultValue(false)
                    .IsRequired();

                // Foreign Key
                entity.HasOne(m => m.User).WithMany(u => u.ReceivedMessages)
                    .HasForeignKey(m => m.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(m => m.Sender).WithMany(u => u.SendMessages)
                    .HasForeignKey(m => m.SenderId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<M2mUsersCamerasEntity>(entity =>
            {
                // Table Name
                entity.ToTable("m2m_Users_Cameras");

                // Primary Key
                entity.HasKey(e => new { e.UserId, e.CameraName });

                // Alternate Key, Indexes


                // Other columns: Name, limit, defaultValue, autoIncriment
                entity.Property(e => e.UserId)
                    .HasColumnName("uc_userId");

                entity.Property(m => m.CameraName)
                    .HasColumnName("uc_cameraName");

                entity.Property(m => m.Favorite)
                    .HasColumnName("uc_favorite")
                    .HasDefaultValue(false)
                    .IsRequired();

                entity.Property(m => m.MotionAlarm)
                    .HasColumnName("uc_motionAlarm")
                    .HasDefaultValue(false)
                    .IsRequired();

                // Foreign Key
                entity.HasOne(e => e.User).WithMany(u => u.M2mUsersCameras)
                    .HasForeignKey(m => m.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Camera).WithMany(c => c.M2MUsersCameras)
                    .HasForeignKey(m => m.CameraName)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private string CreateHashCode(string input)
        {
            string hash = string.Empty;
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                hash = builder.ToString();
            }
            return hash;
        }
    }
}
