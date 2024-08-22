namespace RDF.GL.Domain;

public class UserRole
{
    public int Id { get; set; }
    public string? UserRoleName { get; set; }
    public List<string>? Permissions { get; set; }
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public int AddedBy { get; set; }
    public int ModifiedBy { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<Users>? Users { get; set; }
    public virtual Users? AddedByUser { get; set; }
    public virtual Users? ModifiedByUser { get; set; }
}
