using System.Text.Json.Serialization;

namespace BillingManager.Domain.Utils;

/// <summary>
/// Paged list type
/// </summary>
public class PagedList<T> : List<T>
{
    [JsonPropertyName("current_page")]
    public int CurrentPage { get; }
    
    [JsonPropertyName("total_pages")]
    public int TotalPages { get; }
    
    [JsonPropertyName("page_size")]
    public int PageSize { get; }
    
    [JsonPropertyName("total_count")]
    public int TotalCount { get; }
    
    [JsonPropertyName("has_previous")]
    public bool HasPrevious => CurrentPage > 1;
    
    [JsonPropertyName("has_next")]
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