namespace Tgm3Visualizer.Core.Calculations;

/// <summary>
/// Calculates section times and formats time displays
/// </summary>
public static class SectionTimeCalculator
{
    /// <summary>
    /// Format frames as MM:SS:ss (minutes:seconds:centiseconds)
    /// </summary>
    public static string FormatTime(int frames)
    {
        if (frames <= 0)
            return "";

        var minutes = frames / 3600;
        var seconds = (frames % 3600) / 60;
        var centiseconds = (frames % 60) * 100 / 60;

        return $"{minutes:D2}:{seconds:D2}:{centiseconds:D2}";
    }

    /// <summary>
    /// Get section range display text.
    /// Supports Master mode (0-9, 0-999) and Shirase mode (0-12, 0-1299)
    /// </summary>
    public static string GetSectionRange(int section)
    {
        if (section < 0 || section > 12)
            return "???";

        var start = section * 100;
        // Master mode: section 9 ends at 999
        var end = section == 9 ? 999 : (section + 1) * 100;
        return $"{start:D3}-{end:D3}";
    }
}
