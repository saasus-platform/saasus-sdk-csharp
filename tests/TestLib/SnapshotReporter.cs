using System.Net;
using System.Text;

namespace SaasusSdk.Tests.TestLib;

/// <summary>Renders a snapshot comparison as Markdown, JSON or a self-contained HTML page.</summary>
public sealed class SnapshotReporter
{
    public string ToMarkdown(string story, SnapshotComparison comparison)
    {
        var builder = new StringBuilder($"# Snapshot comparison: {story}\n\n")
            .AppendLine($"Tags: `{comparison.OldTag}` → `{comparison.NewTag}`")
            .AppendLine($"Compatibility: **{Describe(comparison.Level)}**")
            .AppendLine($"Differences: {comparison.Issues.Count}, breaking: {comparison.Summary.BreakingChanges}, warnings: {comparison.Summary.Warnings}\n");
        foreach (var issue in comparison.Issues)
            builder.AppendLine($"- **{Describe(issue.Level)}** `{issue.Path}`: {issue.Description}");
        return builder.ToString();
    }

    public string ToJson(SnapshotComparison comparison) => SnapshotJson.Serialize(comparison);

    /// <summary>Writes the paired JSON document for a report, including the validation artifact.</summary>
    public string ToReportJson(SnapshotComparison comparison, SnapshotValidation? validation = null) =>
        SnapshotJson.Serialize(new { comparison, validation });

    public string ToHtml(SnapshotComparison comparison, SnapshotValidation? validation = null)
    {
        var rows = comparison.Issues.Count == 0
            ? "<tr><td colspan=4>No differences</td></tr>"
            : string.Join("", comparison.Issues.Select(issue =>
                $"<tr><td class=\"impact-{Describe(issue.Level)}\">{E(Describe(issue.Level))}</td><td>{E(issue.Type)}</td><td><code>{E(issue.Path)}</code></td><td>{E(issue.Description)}</td></tr>"));
        var validationText = validation is null
            ? ""
            : $"<h2>Validation</h2><p>Valid: {validation.IsValid}; completion: {E(validation.CompletionStatus)}; " +
              $"errors: {validation.Summary.TotalErrors}; warnings: {validation.Summary.TotalWarnings}; " +
              $"info: {validation.Summary.TotalInfo}</p>{HistoryText(validation.Comparison)}";
        var recommendation = comparison.Level switch
        {
            CompatibilityLevel.Breaking => "Review breaking changes before release.",
            CompatibilityLevel.Warning => "Review warnings and confirm compatibility.",
            _ => "No compatibility action is required."
        };
        return $"<!doctype html><html><head><meta charset=utf-8><title>{E(comparison.StoryName)}</title>" +
               "<style>body{font-family:system-ui;margin:2rem}table{border-collapse:collapse;width:100%}" +
               "td,th{border:1px solid #ccc;padding:.5rem}.impact-breaking{color:#b00020;font-weight:600}" +
               ".impact-warning{color:#8a6d00}.impact-compatible{color:#1b5e20}</style></head><body>" +
               $"<h1>{E(comparison.StoryName)}</h1><p>{E(comparison.OldTag)} → {E(comparison.NewTag)}</p>" +
               $"<p>Compatibility: <strong>{E(Describe(comparison.Level))}</strong></p>" +
               $"<p>Differences: {comparison.Summary.Differences}; breaking: {comparison.Summary.BreakingChanges}; warnings: {comparison.Summary.Warnings}</p>" +
               $"{validationText}<table><thead><tr><th>Impact</th><th>Type</th><th>Path</th><th>Description</th></tr></thead>" +
               $"<tbody>{rows}</tbody></table><h2>Recommended action</h2><p>{E(recommendation)}</p></body></html>";
    }

    private static string HistoryText(ValidationHistory? history) => history is null
        ? ""
        : $"<p>Compared with <code>{E(history.PreviousFile ?? "")}</code>: " +
          $"new {history.NewFindings?.Count ?? 0}, resolved {history.ResolvedFindings?.Count ?? 0}, " +
          $"error delta {history.ErrorCountDelta:+0;-0;0}, warning delta {history.WarningCountDelta:+0;-0;0}</p>";

    private static string Describe(CompatibilityLevel level) => level.ToString().ToLowerInvariant();

    private static string E(string value) => WebUtility.HtmlEncode(value);
}
