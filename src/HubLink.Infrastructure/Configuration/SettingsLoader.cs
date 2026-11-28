using System.Text.Json;
using HubLink.Application.Configuration;

namespace HubLink.Infrastructure.Configuration;

/// <summary>
/// Loads settings from appsettings.json (next to the executable), then applies optional
/// environment-variable overrides: HUBLINK_BASE_URL, HUBLINK_SHIPMENTS_PATH, HUBLINK_BRANCH_CODE,
/// HUBLINK_EXTRA_QUERY, HUBLINK_TIMEOUT_SECONDS.
/// </summary>
public static class SettingsLoader
{
    public static HubLinkSettings Load(string? baseDirectory = null)
    {
        var settings = new HubLinkSettings();
        var path = Path.Combine(baseDirectory ?? AppContext.BaseDirectory, "appsettings.json");

        if (File.Exists(path))
        {
            try
            {
                using var doc = JsonDocument.Parse(File.ReadAllText(path),
                    new JsonDocumentOptions { AllowTrailingCommas = true, CommentHandling = JsonCommentHandling.Skip });

                if (doc.RootElement.TryGetProperty("HubLink", out var section))
                {
                    settings = JsonSerializer.Deserialize<HubLinkSettings>(
                        section.GetRawText(),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? settings;
                }
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException($"appsettings.json is not valid JSON: {ex.Message}", ex);
            }
        }

        settings.BaseUrl = Env("HUBLINK_BASE_URL") ?? settings.BaseUrl;
        settings.ShipmentsPath = Env("HUBLINK_SHIPMENTS_PATH") ?? settings.ShipmentsPath;
        settings.BranchCode = Env("HUBLINK_BRANCH_CODE") ?? settings.BranchCode;
        settings.ExtraQuery = Env("HUBLINK_EXTRA_QUERY") ?? settings.ExtraQuery;
        if (int.TryParse(Env("HUBLINK_TIMEOUT_SECONDS"), out var seconds) && seconds > 0)
            settings.TimeoutSeconds = seconds;

        return settings;
    }

    private static string? Env(string name)
    {
        var value = Environment.GetEnvironmentVariable(name);
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
