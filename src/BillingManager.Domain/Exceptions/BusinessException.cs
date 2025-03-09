namespace BillingManager.Domain.Exceptions;

/// <summary>
/// Exception for business rules
/// </summary>
public class BusinessException(string errorCode, string message) : Exception(message)
{
    public string ErrorCode { get; } = errorCode;
}