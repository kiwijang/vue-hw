using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public partial class VueHwDbContext : IdentityDbContext<AppUser, AppRole, int,
        IdentityUserClaim<int>, IdentityUserRole<int>, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
{
    public VueHwDbContext(DbContextOptions<VueHwDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Eventjoinuser> Eventjoinusers { get; set; }

    public virtual DbSet<AppUser> AppUsers { get; set; }
    public virtual DbSet<AppUser> AppRole { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // modelBuilder.Entity<Aspnetuser>(entity =>
        // {
        //     entity.HasKey(e => e.Id).HasName("PRIMARY");

        //     entity.ToTable("aspnetusers");

        //     entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

        //     entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex").IsUnique();

        //     entity.Property(e => e.Email).HasMaxLength(256);
        //     entity.Property(e => e.LockoutEnd).HasMaxLength(6);
        //     entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
        //     entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
        //     entity.Property(e => e.UserName).HasMaxLength(256);
        // });

        // modelBuilder.Entity<Aspnetuserclaim>(entity =>
        // {
        //     entity.HasKey(e => e.Id).HasName("PRIMARY");

        //     entity.ToTable("aspnetuserclaims");

        //     entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

        //     entity.HasOne(d => d.User).WithMany(p => p.Aspnetuserclaim)
        //         .HasForeignKey(d => d.UserId)
        //         .HasConstraintName("FK_AspNetUserClaims_AspNetUsers_UserId");
        // });

        // modelBuilder.Entity<Aspnetuserlogin>(entity =>
        // {
        //     entity.HasKey(e => new { e.LoginProvider, e.ProviderKey }).HasName("PRIMARY");
        // });

        // modelBuilder.Entity<Aspnetusertoken>(entity =>
        // {
        //     entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name }).HasName("PRIMARY");
        // });

        // modelBuilder.Entity<IdentityRoleClaim<string>>(b =>
        // {
        //     // Primary key
        //     b.HasKey(rc => rc.Id);

        //     // Maps to the AspNetRoleClaims table
        //     b.ToTable("aspnetroleclaims");
        // });

        // modelBuilder.Entity<AppUserRole<string>>(b =>
        // {
        //     // Primary key
        //     b.HasKey(r => new { r.UserId, r.RoleId });

        //     // Maps to the AspNetUserRoles table
        //     b.ToTable("aspnetuserroles");
        // });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("event");

            entity.Property(e => e.Address).HasMaxLength(6);
            entity.Property(e => e.Cost).HasMaxLength(6);
            entity.Property(e => e.EndTime).HasMaxLength(6);
            entity.Property(e => e.StartTime).HasMaxLength(6);
        });

        modelBuilder.Entity<Eventjoinuser>(entity =>
        {
            entity.HasKey(e => new { e.EventId, e.UserId }).HasName("PRIMARY");

            entity.ToTable("eventjoinuser");

            entity.HasOne(d => d.Event).WithMany(p => p.Eventjoinusers)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK_EventJoinUser_Event_EventId");
        });

        // modelBuilder.Entity<AppUser>(entity =>
        // {
        //     entity.HasKey(e => e.Id).HasName("PRIMARY");

        //     entity.ToTable("AppUser");
        // });

        // modelBuilder.Entity<AppUserRole>()
        //     .HasKey(ur => new { ur.RoleId, ur.UserId });

        // modelBuilder.Entity<AppRole>()
        //     .HasMany(ur => ur.UserRoles)
        //     .WithOne(u => u.Role)
        //     .HasForeignKey(ur => ur.RoleId)
        //     .IsRequired();
    }

}
