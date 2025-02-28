namespace BillingManager.Domain.Utils;

/// <summary>
/// Paged list type
/// </summary>
public class PagedList<T> : List<T>
{
    public int CurrentPage { get; }
    
    public int TotalPages { get; }
    
    public int PageSize { get; }
    
    public int TotalCount { get; }
    
    public bool HasPrevious => CurrentPage > 1;
    
    public bool HasNext => CurrentPage < TotalPages;
    
    public PagedList(IQueryable<T> source, int pageNumber, int pageSize)
    {
        TotalCount = source.Count();
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

        var items = source.Skip((CurrentPage - 1) * PageSize)
            .Take(PageSize)
            .ToList();

        AddRange(items);
    }
    
    public PagedList(IEnumerable<T> source, int pageNumber, int pageSize, int totalCount)
    {
        TotalCount = totalCount;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(TotalCount / (double)this.PageSize);
        
        AddRange(source);
    }
}