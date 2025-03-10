using BackEnd.DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Tables.Context
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

        public DbSet<M2mUsersFoldersEntity> M2mUsersFoldersEntity { get; set; } = null!;

        public DbSet<CamerasEntity> Cameras { get; set; } = null!;

        public DbSet<PresetsEntity> Presets { get; set; } = null!;

        public DbSet<EventsEntity> Events { get; set; } = null!;

        public DbSet<AgentsEntity> Agents { get; set; } = null!;

        public DbSet<StreamersEntity> Streamers { get; set; } = null!;

        public DbSet<MosaicsEntity> Mosaics { get; set; } = null!;  

        public DbSet<M2mMosaicsCamerasEntity> M2mMosaicsCameras { get; set; } = null!;

        public DbSet<MessagesEntity> Messages { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsersEntity>(entity =>
            {
                // Table Name
                entity.ToTable("Users");

                // Primary Key
                entity.HasKey(u => u.Id);

                // Indexes
                entity.HasIndex(u => u.Login, "usr_login_UNIQUE").IsUnique();
                entity.HasIndex(u => u.Email, "usr_email_UNIQUE").IsUnique();

                // Other columns: FK, Name, limit, defaultValue, autoIncriment
                entity.Property(u => u.Id)
                    .HasColumnName("usr_id")
                    .UseIdentityColumn();
                entity.Property(u => u.Name)
                    .HasColumnName("usr_name")
                    .HasMaxLength(50);
                entity.Property(u => u.Login)
                    .HasColumnName("usr_login")
                    .HasMaxLength(50);
                entity.Property(u => u.Password)
                    .HasColumnName("usr_password");
                entity.Property(u => u.Email)
                    .HasColumnName("usr_email")
                    .HasMaxLength(100);

                // Foreign Key

            });

            modelBuilder.Entity<OrganizationsEntity>(entity =>
            {
                // Table Name
                entity.ToTable("Organizations");

                // Primary Key
                entity.HasKey(o => o.Id);

                // Indexes
                entity.HasIndex(o => o.Title, "org_title_UNIQUE").IsUnique();

                // Other columns: Name, limit, defaultValue, autoIncriment
                entity.Property(o => o.Id)
                    .HasColumnName("org_id")
                    .UseIdentityColumn();
                entity.Property(o => o.Title)
                    .HasColumnName("org_title")
                    .HasMaxLength(50);

                // Foreign Key
                
            });

            modelBuilder.Entity<M2mUsersOrganizationsEntity>(entity =>
            {
                // Table Name
                entity.ToTable("m2m_Users_Organizations");

                // Primary Key
                entity.HasKey(e => e.Id);

                // Indexes
                entity.HasIndex(e => e.UserId, "fk_m2m_Users_Organizations_usr_idx");
                entity.HasIndex(e => e.OrganizationId, "fk_m2m_Users_Organizations_org_idx");

                // Other columns: Name, limit, defaultValue, autoIncriment
                entity.Property(e => e.Id)
                    .HasColumnName("uo_id")
                    .UseIdentityColumn();
                entity.Property(e => e.UserId)
                    .HasColumnName("uo_usr_id");
                entity.Property(e => e.OrganizationId)
                    .HasColumnName("uo_org_id");

                // Foreign Key
                entity.HasOne(e => e.User).WithMany(u => u.M2mUsersOrganizations)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_m2m_Users_Organizations_usr");
                entity.HasOne(e => e.Organization).WithMany(o => o.M2mUsersOrganizations)
                    .HasForeignKey(e => e.OrganizationId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_m2m_Users_Organizations_org");
            });

            modelBuilder.Entity<FoldersEntity>(entity =>
            {
                // Table Name
                entity.ToTable("Folders");

                // Primary Key
                entity.HasKey(f => f.Id);

                // Indexes
                entity.HasIndex(f => f.OrganizationId, "fk_Folders_org_idx");
                entity.HasIndex(f => f.ParentsId, "fk_Folders_parents_idx");

                // Other columns: FK, Name, limit, defaultValue, autoIncriment
                entity.Property(f => f.Id)
                    .HasColumnName("fld_id")
                    .UseIdentityColumn();
                entity.Property(f => f.OrganizationId)
                    .HasColumnName("fld_org_id");
                entity.Property(f => f.ParentsId)
                    .HasColumnName("fld_parents_id");

                // Foreign Key
                entity.HasOne(f => f.Organization).WithMany(o => o.Folders)
                    .HasForeignKey(f => f.OrganizationId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_Folders_org");
                entity.HasOne(f => f.ParentsFolder).WithMany(f => f.ChildFolders)
                    .HasForeignKey(f => f.ParentsId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_Folders_parents");
            });

            modelBuilder.Entity<M2mUsersFoldersEntity>(entity =>
            {
                // Table Name
                entity.ToTable("m2m_Users_Folders");

                // Primary Key
                entity.HasKey(e => e.Id);

                // Indexes
                entity.HasIndex(e => e.UserId, "fk_m2m_Users_Folders_usr_idx");
                entity.HasIndex(e => e.FolderId, "fk_m2m_Users_Folders_fld_idx");

                // Other columns: Name, limit, defaultValue, autoIncriment
                entity.Property(e => e.Id)
                    .HasColumnName("uf_id")
                    .UseIdentityColumn();
                entity.Property(e => e.UserId)
                    .HasColumnName("uf_usr_id");
                entity.Property(e => e.FolderId)
                    .HasColumnName("uf_fld_id");

                // Foreign Key
                entity.HasOne(e => e.User).WithMany(u => u.M2mUsersFolders)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_m2m_Users_Folders_usr");
                entity.HasOne(e => e.Folder).WithMany(o => o.M2mUsersFolders)
                    .HasForeignKey(e => e.FolderId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_m2m_Users_Folder_fld");
            });

            modelBuilder.Entity<CamerasEntity>(entity =>
            {
                // Table Name
                entity.ToTable("Cameras");

                // Primary Key
                entity.HasKey(c => c.Id);

                // Indexes
                entity.HasIndex(c => c.FolderId, "fk_Cameras_fld_idx");
                entity.HasIndex(c => c.PresetId, "fk_Cameras_prst_idx");
                entity.HasIndex(c => c.AgentId, "fk_Cameras_agnt_idx");
                entity.HasIndex(c => c.StreamerId, "fk_Cameras_strm_idx");


                // Other columns: FK, Name, limit, defaultValue, autoIncriment
                entity.Property(c => c.Id)
                    .HasColumnName("cam_id")
                    .UseIdentityColumn();
                entity.Property(c => c.FolderId)
                    .HasColumnName("cam_fld_id");
                entity.Property(c => c.PresetId)
                    .HasColumnName("cam_prst_id");
                entity.Property(c => c.AgentId)
                    .HasColumnName("cam_agnt_id");
                entity.Property(c => c.StreamerId)
                    .HasColumnName("cam_strm_id");

                // Foreign Key
                entity.HasOne(c => c.Folder).WithMany(f => f.Cameras)
                    .HasForeignKey(c => c.FolderId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_Cameras_fld");
                entity.HasOne(c => c.Preset).WithMany(p => p.Cameras)
                    .HasForeignKey(c => c.PresetId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_Cameras_prst");
                entity.HasOne(c => c.Agent).WithMany(a => a.Cameras)
                    .HasForeignKey(c => c.AgentId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_Cameras_agnt");
                entity.HasOne(c => c.Streamer).WithMany(s => s.Cameras)
                    .HasForeignKey(c => c.StreamerId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_Cameras_strm");
            });

            modelBuilder.Entity<PresetsEntity>(entity =>
            {
                // Table Name
                entity.ToTable("Presets");

                // Primary Key
                entity.HasKey(p => p.Id);

                // Indexes
                

                // Other columns: FK, Name, limit, defaultValue, autoIncriment
                entity.Property(p => p.Id)
                      .HasColumnName("prst_id")
                      .UseIdentityColumn();

                // Foreign Key

            });

            modelBuilder.Entity<EventsEntity>(entity =>
            {
                // Table Name
                entity.ToTable("Enents");

                // Primary Key
                entity.HasKey(e => e.Id);

                // Indexes
                entity.HasIndex(e => e.CameraId, "fk_Events_cam_idx");

                // Other columns: FK, Name, limit, defaultValue, autoIncriment
                entity.Property(e => e.Id)
                      .HasColumnName("envt_id")
                      .UseIdentityColumn();
                entity.Property(e => e.CameraId)
                      .HasColumnName("envt_cam_id");

                // Foreign Key
                entity.HasOne(e => e.Camera).WithMany(c => c.Events)
                    .HasForeignKey(e => e.CameraId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_Events_cam");

            });

            modelBuilder.Entity<AgentsEntity>(entity =>
            {
                // Table Name
                entity.ToTable("Agents");

                // Primary Key
                entity.HasKey(a => a.Id);

                // Indexes


                // Other columns: FK, Name, limit, defaultValue, autoIncriment
                entity.Property(a => a.Id)
                      .HasColumnName("agnt_id")
                      .UseIdentityColumn();

                // Foreign Key

            });

            modelBuilder.Entity<StreamersEntity>(entity =>
            {
                // Table Name
                entity.ToTable("Streamers");

                // Primary Key
                entity.HasKey(s => s.Id);

                // Indexes


                // Other columns: FK, Name, limit, defaultValue, autoIncriment
                entity.Property(s => s.Id)
                      .HasColumnName("strm_id")
                      .UseIdentityColumn();

                // Foreign Key

            });

            modelBuilder.Entity<MosaicsEntity>(entity =>
            {
                // Table Name
                entity.ToTable("Mosaics");

                // Primary Key
                entity.HasKey(m => m.Id);

                // Indexes
                entity.HasIndex(m => m.OrganizationId, "fk_Mosaics_org_idx");

                // Other columns: FK, Name, limit, defaultValue, autoIncriment
                entity.Property(m => m.Id)
                      .HasColumnName("mosc_id")
                      .UseIdentityColumn();
                entity.Property(m => m.OrganizationId)
                      .HasColumnName("mosc_org_id");

                // Foreign Key
                entity.HasOne(m => m.Organization).WithMany(o => o.Mosaics)
                    .HasForeignKey(m => m.OrganizationId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_Mosaics_org");
            });

            modelBuilder.Entity<M2mMosaicsCamerasEntity>(entity =>
            {
                // Table Name
                entity.ToTable("m2m_Mosaics_Cameras");

                // Primary Key
                entity.HasKey(e => e.Id);

                // Indexes
                entity.HasIndex(e => e.MosaicId, "fk_m2m_Mosaics_Cameras_mosc_idx");
                entity.HasIndex(e => e.CameraId, "fk_m2m_Mosaics_Cameras_cam_idx");

                // Other columns: Name, limit, defaultValue, autoIncriment
                entity.Property(e => e.Id)
                    .HasColumnName("mc_id")
                    .UseIdentityColumn();
                entity.Property(e => e.MosaicId)
                    .HasColumnName("mc_mosc_id");
                entity.Property(e => e.CameraId)
                    .HasColumnName("mc_cam_id");

                // Foreign Key
                entity.HasOne(e => e.Mosaic).WithMany(u => u.M2mMosaicsCameras)
                    .HasForeignKey(e => e.MosaicId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_m2m_Mosaics_Cameras_mosc");
                entity.HasOne(e => e.Camera).WithMany(o => o.M2mMosaicsCameras)
                    .HasForeignKey(e => e.CameraId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_m2m_Mosaics_Cameras_cam");

            });

            modelBuilder.Entity<MessagesEntity>(entity =>
            {
                // Table Name
                entity.ToTable("Messages");

                // Primary Key
                entity.HasKey(m => m.Id);

                // Indexes
                entity.Property(m => m.Id)
                    .HasColumnName("mess_id")
                    .UseIdentityColumn();

                // Other columns: FK, Name, limit, defaultValue, autoIncriment


                // Foreign Key

            });
        }
    }
}
