using System.Data;

namespace RDF.GL.Domain;

public class Users
{
    public int Id { get; set; }
    public string IdPrefix { get; set; }
    public string IdNumber { get; set; }
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Sex { get; set; }
    public int? UserRoleId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    public virtual UserRole? UserRole { get; set; }
}
