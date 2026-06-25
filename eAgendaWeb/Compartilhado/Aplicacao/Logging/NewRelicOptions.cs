namespace eAgendaWeb.Compartilhado.Aplicacao.Logging;

public sealed class NewRelicOptions
{
    public const string SectionName = "NewRelic";

    public string EndpointUrl { get; set; } = string.Empty;
    public string ApplicationName { get; set; } = string.Empty;
    public string? LicenseKey { get; set; }
}
