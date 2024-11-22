using Microsoft.EntityFrameworkCore;
using RDF.GL.Domain;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace RDF.GL.Data;

public class ProjectGLDbContext :DbContext
{
    public ProjectGLDbContext(DbContextOptions<ProjectGLDbContext> options) : base(options) { }

    public virtual DbSet<Users> Users { get; set; }
    public virtual DbSet<UserRole> UserRoles { get; set; }
    public virtual DbSet<GeneralLedger> GeneralLedgers { get; set; }
    public virtual DbSet<Domain.System> Systems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed the Users table
        modelBuilder.Entity<Users>().HasData(new Users
        {
            Id = 1,
            FirstName = "Admin",
            Username = "admin",
            Password = BCrypt.Net.BCrypt.HashPassword("admin"),
            IsActive = true,
            CreatedAt = DateTime.Now,
            UserRoleId = 1
        });

        // Seed the UserRole table with the same user id
        modelBuilder.Entity<UserRole>().HasData(new UserRole
        {
            Id = 1,
            UserRoleName = "Admin",
            UpdatedAt = DateTime.UtcNow,
            Permissions = ["User Management"]
        });

        modelBuilder.Entity<UserRole>()
            .HasOne(x => x.AddedByUser)
            .WithMany()
            .HasForeignKey(x => x.AddedBy);

        modelBuilder.Entity<UserRole>()
            .HasOne(x => x.ModifiedByUser)
            .WithMany()
            .HasForeignKey(x => x.ModifiedBy);

        modelBuilder.Entity<GeneralLedger>()
            .HasOne(x => x.AddedByUser)
            .WithMany()
            .HasForeignKey(x => x.AddedBy);


        

    }

}
