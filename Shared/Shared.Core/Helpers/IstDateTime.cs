namespace Shared.Core.Helpers;

/// <summary>
/// Every timestamp in this system is stored and compared in UTC (SaleDate,
/// CreatedOn, etc. are always <see cref="DateTime.UtcNow"/>), but the shop
/// operates on India Standard Time business days. "Today"/"this month"
/// boundaries computed from <see cref="DateTime.UtcNow"/> directly land on
/// the wrong calendar day for anything that happens in the ~5:30 window
/// where UTC and IST disagree on the date. This helper computes those
/// boundaries as real UTC instants that correctly correspond to IST
/// midnight, so callers can keep comparing/storing pure UTC everywhere.
/// India does not observe daylight saving, so a fixed offset is always correct.
/// </summary>
public static class IstDateTime
{
    public static readonly TimeSpan Offset = new(5, 30, 0);

    /// <summary>The current instant, in IST, used only to read off calendar components (Year/Month/Day) - not a real UTC instant.</summary>
    public static DateTime NowIst => DateTime.UtcNow.Add(Offset);

    /// <summary>The UTC instant corresponding to 00:00 IST on the current calendar day.</summary>
    public static DateTime TodayStartUtc()
    {
        var nowIst = NowIst;
        var istMidnight = new DateTime(nowIst.Year, nowIst.Month, nowIst.Day, 0, 0, 0, DateTimeKind.Utc);
        return istMidnight - Offset;
    }

    /// <summary>The UTC instant corresponding to 00:00 IST on the first day of the current month.</summary>
    public static DateTime MonthStartUtc()
    {
        var nowIst = NowIst;
        var istMonthStart = new DateTime(nowIst.Year, nowIst.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        return istMonthStart - Offset;
    }
}
