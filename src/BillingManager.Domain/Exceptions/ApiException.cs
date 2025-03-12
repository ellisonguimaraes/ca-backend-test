using BillingManager.Domain.Resources;

namespace BillingManager.Domain.Exceptions;

/// <summary>
/// Exception for external API errors
/// </summary>
public class ApiException(HttpResponseMessage response)
    : Exception(string.Format(ErrorsResource.EXTERNAL_API_RETURN_UNSUCCESFULLY_STATUSCODE_MESSAGE, response.RequestMessage?.Method, response.RequestMessage?.RequestUri, (int)response.StatusCode))
{
    public string ErrorCode { get; } = ErrorsResource.EXTERNAL_API_RETURN_UNSUCCESFULLY_STATUSCODE_CODE;
    public HttpResponseMessage HttpResponseMessage { get; } = response;
}