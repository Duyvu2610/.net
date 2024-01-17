using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace hrm_server.entities;

public partial class HrmSystemContext : DbContext
{
    public HrmSystemContext()
    {
    }

    public HrmSystemContext(DbContextOptions<HrmSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=localhost;Database=hrm_system;User Id=postgres;Password=admin");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>();
        modelBuilder.Entity<Tokens>(entity =>
        {
            entity.HasOne(d => d.User).WithMany(p => p.Tokens)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("token_user_id_fkey");
        });
        modelBuilder.Entity<ProjectEmp>(entity =>
        {
            entity.HasOne(d => d.User).WithMany(p => p.ProjectsEmps)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("project_emp_user_id_fkey");
        });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
