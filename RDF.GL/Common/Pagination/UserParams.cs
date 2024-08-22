namespace RDF.GL.Common.Pagination;

public class UserParams
{
    private const int MaxPageSize = 100000;
    public int PageNumber { get; set; } = 1;
    private int _pageSize = 10;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

}
