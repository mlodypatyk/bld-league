namespace BldLeague.Application.Common;

/// <summary>
/// Resolves round timing semantics in the configured league time zone (default Europe/Warsaw).
/// Round StartDate/EndDate are bound from a date picker (time component is always 00:00),
/// so only the calendar date matters: a round is active for the full extent of its end day
/// in local time, regardless of UTC offset or DST.
/// </summary>
public class RoundClock(TimeZoneInfo timeZone)
{
    public DateTime LocalToday()
        => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone).Date;

    public bool IsRoundFinished(DateTime endDate)
        => LocalToday() > endDate.Date;

    public bool IsRoundActive(DateTime startDate, DateTime endDate)
    {
        var today = LocalToday();
        return today >= startDate.Date && today <= endDate.Date;
    }
}
