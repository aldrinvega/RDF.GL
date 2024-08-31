using Microsoft.EntityFrameworkCore;
using RDF.GL.Domain;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace RDF.GL.Data;

public class ProjectGLDbContext :DbContext
{
    public ProjectGLDbContext(DbContextOptions<ProjectGLDbContext> options) : base(options) { }

    public virtual DbSet<Users> Users { get; set; }
    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserRole>()
            .HasOne(x => x.AddedByUser)
            .WithMany()
            .HasForeignKey(x => x.AddedBy);

        modelBuilder.Entity<UserRole>()
            .HasOne(x => x.ModifiedByUser)
            .WithMany()
            .HasForeignKey(x => x.ModifiedBy);

        modelBuilder.Entity<Users>().HasData(new Users
        {
            Id = 1,
            Username = "admin",
            Password = BCrypt.Net.BCrypt.HashPassword("admin"),
            IsActive = true,
            CreatedAt = DateTime.Now
        });
        modelBuilder.Entity<UserRole>().HasData(new UserRole
        {
            Id = 1,
            UserRoleName = "Admin",
            AddedBy = 1,
            ModifiedBy = 1,
            UpdatedAt = DateTime.UtcNow
        });

    }

}
