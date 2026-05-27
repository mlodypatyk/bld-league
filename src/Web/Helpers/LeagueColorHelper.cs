namespace BldLeague.Web.Helpers;

public static class LeagueColorHelper
{
    private const string Fallback = "var(--bs-secondary)";

    private static readonly string[] Palette =
    [
        "var(--bs-primary)",
        "var(--bs-success)",
        "var(--bs-danger)",
        "var(--bs-warning)",
        "#ed8257",
        "var(--bs-purple)",
    ];

    public static string GetBackgroundColor(string? leagueIdentifier)
    {
        if (string.IsNullOrEmpty(leagueIdentifier))
            return Fallback;

        var first = char.ToUpperInvariant(leagueIdentifier[0]);
        if (first < 'A' || first > 'Z')
            return Fallback;

        return Palette[(first - 'A') % Palette.Length];
    }
}
