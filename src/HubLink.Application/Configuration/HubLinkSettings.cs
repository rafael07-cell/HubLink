namespace HubLink.Application.Configuration;

/// <summary>Where and how HubLink talks to the shipment service. Changing these never requires code changes.</summary>
public class HubLinkSettings
{
    public string BaseUrl { get; set; } = "http://127.0.0.1:5080";
    public string ShipmentsPath { get; set; } = "/api/shipments";
    public string BranchCode { get; set; } = "BR-001";

    /// <summary>Optional extra query text, for example "mode=delay" when using the test endpoint.</summary>
    public string ExtraQuery { get; set; } = "";

    public int TimeoutSeconds { get; set; } = 15;

    public Uri BuildShipmentsUri()
    {
        if (!Uri.TryCreate(BaseUrl, UriKind.Absolute, out var baseUri) ||
            (baseUri.Scheme != Uri.UriSchemeHttp && baseUri.Scheme != Uri.UriSchemeHttps))
        {
            throw new InvalidOperationException($"BaseUrl '{BaseUrl}' is not a valid http(s) address.");
        }

        if (string.IsNullOrWhiteSpace(BranchCode))
            throw new InvalidOperationException("BranchCode must not be empty.");

        var path = ShipmentsPath.StartsWith('/') ? ShipmentsPath : "/" + ShipmentsPath;
        var query = $"branchCode={Uri.EscapeDataString(BranchCode)}";
        if (!string.IsNullOrWhiteSpace(ExtraQuery))
            query += "&" + ExtraQuery.TrimStart('&', '?');

        return new UriBuilder(baseUri) { Path = path, Query = query }.Uri;
    }
}
