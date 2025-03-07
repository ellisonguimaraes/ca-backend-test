using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace BillingManager.Domain.Utils;

/// <summary>
/// Pagination parameters model
/// </summary>
public class PaginationParameters
{
    #region Constants
    private const int MAX_PAGE_SIZE = 50;
    #endregion
    
    private int PAGE_NUMBER { get; set; }
    private int PAGE_SIZE { get; set; }

    [FromQuery(Name = "page_number")]
    [JsonPropertyName("page_number")]
    public int PageNumber 
    { 
        get => PAGE_NUMBER; 
        set => PAGE_NUMBER = value > 0? value : 1;
    }

    [FromQuery(Name = "page_size")]
    [JsonPropertyName("page_size")]
    public int PageSize
    { 
        get => PAGE_SIZE; 
        set {
            PAGE_SIZE = value switch
            {
                > MAX_PAGE_SIZE => MAX_PAGE_SIZE,
                <= 0 => 1,
                _ => value
            };
        }
    }
}