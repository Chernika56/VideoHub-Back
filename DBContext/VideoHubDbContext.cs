using System;
using System.Collections.Generic;
using BackEnd.Models;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace BackEnd.DBContext;

public partial class VideoHubDbContext : DbContext
{
    public VideoHubDbContext()
    {
    }

    public VideoHubDbContext(DbContextOptions<VideoHubDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Camera> Cameras { get; set; }

    public virtual DbSet<M2mCameraUser> M2mCameraUsers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Camera>(entity =>
        {
            entity.HasKey(e => e.CamId).HasName("PRIMARY");

            entity.ToTable("cameras");

            entity.HasIndex(e => e.CamUrl, "cam_link_UNIQUE").IsUnique();

            entity.Property(e => e.CamId).HasColumnName("cam_id");
            entity.Property(e => e.CamArchiveTime).HasColumnName("cam_archive_time");
            entity.Property(e => e.CamIp)
                .HasMaxLength(20)
                .HasColumnName("cam_ip")
                .UseCollation("utf8mb4_general_ci");
            entity.Property(e => e.CamName)
                .HasMaxLength(50)
                .HasColumnName("cam_name");
            entity.Property(e => e.CamType)
                .HasMaxLength(45)
                .HasColumnName("cam_type");
            entity.Property(e => e.CamUrl)
                .HasMaxLength(100)
                .HasColumnName("cam_url")
                .UseCollation("utf8mb4_general_ci");
        });

        modelBuilder.Entity<M2mCameraUser>(entity =>
        {
            entity.HasKey(e => new { e.CuId, e.CuUsrId, e.CuCamId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity.ToTable("m2m_camera_user");

            entity.HasIndex(e => e.CuCamId, "fk_m2m_Camera_User_Cameras_idx");

            entity.HasIndex(e => e.CuUsrId, "fk_m2m_Camera_User_Users1_idx");

            entity.Property(e => e.CuId)
                .ValueGeneratedOnAdd()
                .HasColumnName("cu_id");
            entity.Property(e => e.CuUsrId).HasColumnName("cu_usr_id");
            entity.Property(e => e.CuCamId).HasColumnName("cu_cam_id");

            entity.HasOne(d => d.CuCam).WithMany(p => p.M2mCameraUsers)
                .HasForeignKey(d => d.CuCamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_m2m_Camera_User_Cameras");

            entity.HasOne(d => d.CuUsr).WithMany(p => p.M2mCameraUsers)
                .HasForeignKey(d => d.CuUsrId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_m2m_Camera_User_Users1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UsrId).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.UsrLogin, "usr_login_UNIQUE").IsUnique();

            entity.Property(e => e.UsrId).HasColumnName("usr_id");
            entity.Property(e => e.UsrEmail)
                .HasMaxLength(100)
                .HasColumnName("usr_email")
                .UseCollation("utf8mb4_general_ci");
            entity.Property(e => e.UsrLogin)
                .HasMaxLength(45)
                .HasColumnName("usr_login")
                .UseCollation("utf8mb4_general_ci");
            entity.Property(e => e.UsrName)
                .HasMaxLength(45)
                .HasColumnName("usr_name");
            entity.Property(e => e.UsrPassword)
                .HasMaxLength(100)
                .HasColumnName("usr_password")
                .UseCollation("utf8mb4_general_ci");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
