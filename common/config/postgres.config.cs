using Microsoft.EntityFrameworkCore;
using hrm_server.entities;
using Npgsql;

namespace Common.Config;

public partial class PostgresConfig : DbContext
{

  public PostgresConfig(DbContextOptions options) : base(options)
  {
    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
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
    modelBuilder.Entity<RefreshToken>(entity =>
    {
      entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
        .HasForeignKey(d => d.UserId)
        .OnDelete(DeleteBehavior.ClientSetNull)
        .HasConstraintName("refresh_token_user_id_fkey");
    });
    modelBuilder.Entity<ProjectEmp>(entity =>
    {
      entity.HasOne(d => d.User).WithMany(p => p.ProjectsEmps)
        .HasForeignKey(d => d.UserId)
        .OnDelete(DeleteBehavior.ClientSetNull)
        .HasConstraintName("project_emp_user_id_fkey");
    });
    modelBuilder.Entity<Leaves>(entity =>
    {
        entity.HasOne(d => d.User).WithMany(p => p.Leaves)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("leave_user_id_fkey");
    });

    OnModelCreatingPartial(modelBuilder);
  }
  partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

  public DbSet<User> Users { get; set; }
  public DbSet<Tokens> Tokens { get; set; }
  public DbSet<ProjectEmp> ProjectEmps { get; set; }
  public DbSet<Projects> Projects { get; set; }
  public DbSet<RefreshToken> RefreshTokens { get; set; }
  public DbSet<Leaves> Leaves { get; set; }
}

public class PostgresFactory : IDbContextFactory<PostgresConfig>
{

  private readonly IDbContextFactory<PostgresConfig> _pooledFactory;

  public PostgresFactory(IDbContextFactory<PostgresConfig> pooledFactory)
  {
    _pooledFactory = pooledFactory;
  }

  public PostgresConfig CreateDbContext()
  {
    var context = _pooledFactory.CreateDbContext();
    return context;
  }
}