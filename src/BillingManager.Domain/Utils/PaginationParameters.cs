namespace BillingManager.Domain.Utils;

/// <summary>
/// Pagination parameters model
/// </summary>
public class PaginationParameters
{
    private const int MAX_PAGE_SIZE = 50;

    public int PageNumber { get; }

    public int PageSize { get; }

    public PaginationParameters() { }

    public PaginationParameters(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber > 0? pageNumber : 1;

        PageSize = pageSize switch
        {
            > MAX_PAGE_SIZE => MAX_PAGE_SIZE,
            <= 0 => 1,
            _ => pageSize
        };
    }
}