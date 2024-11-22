namespace RDF.GL.Domain;

public class System
{
    public int Id { get; set; }
    public string SystemName { get; set; }
    public string Endpoint  { get; set; }
    public string Token { get; set; }
    public bool IsActive { get; set; } = true;
    
}