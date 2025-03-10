namespace BillingManager.Domain.Configurations;

public class SwaggerInfoSettings
{
    public string Title { get; set; } = null!;
    public string Version { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string RoutePrefix { get; set; } = null!;
    public string SwaggerEndpoint { get; set; } = null!;
    public SwaggerInfoContact Contact { get; set; } = null!;
}

public class SwaggerInfoContact
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Url { get; set; }
}