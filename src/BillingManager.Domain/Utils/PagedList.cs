using System.Text.Json.Serialization;

namespace BillingManager.Domain.Utils;

/// <summary>
/// Paged list type
/// </summary>
public class PagedList<T>
{
    [JsonPropertyName("current_page")]
    public int CurrentPage { get; set; }
    
    [JsonPropertyName("total_pages")]
    public int TotalPages { get; set; }
    
    [JsonPropertyName("page_size")]
    public int PageSize { get; set; }
    
    [JsonPropertyName("total_count")]
    public int TotalCount { get; set; }
    
    [JsonPropertyName("has_previous")]
    public bool HasPrevious => CurrentPage > 1;
    
    [JsonPropertyName("has_next")]
    public bool HasNext => CurrentPage < TotalPages;
    
    [JsonPropertyName("items")]
    public IList<T> Items { get; set; }

    public PagedList()
    {
    }
    
    public PagedList(IQueryable<T> source, int pageNumber, int pageSize)
    {
        TotalCount = source.Count();
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

        var items = source.Skip((CurrentPage - 1) * PageSize)
            .Take(PageSize)
            .ToList();

        Items = items;
    }
    
    public PagedList(IEnumerable<T> source, int pageNumber, int pageSize, int totalCount)
    {
        TotalCount = totalCount;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(TotalCount / (double)this.PageSize);

        Items = source.ToList();
    }
}