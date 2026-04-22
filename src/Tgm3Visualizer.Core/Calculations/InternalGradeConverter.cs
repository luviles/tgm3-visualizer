namespace Tgm3Visualizer.Core.Calculations;

/// <summary>
/// Converts internal grade values (0-31+) to display strings
/// </summary>
public static class InternalGradeConverter
{
    /// <summary>
    /// Maps internal grade value to display string
    /// </summary>
    /// <param name="internalGrade">The internal grade value (0-31+)</param>
    /// <returns>The display string (e.g., "9", "8", "S1", "S9")</returns>
    public static string ToDisplayGrade(int internalGrade)
    {
        return internalGrade switch
        {
            0 => "9",
            1 => "8",
            2 => "7",
            3 => "6",
            4 => "5",
            5 => "4",
            6 => "4",
            7 => "3",
            8 => "3",
            9 => "2",
            10 => "2",
            11 => "2",
            12 => "1",
            13 => "1",
            14 => "1",
            15 => "S1",
            16 => "S1",
            17 => "S1",
            18 => "S2",
            19 => "S3",
            20 => "S4",
            21 => "S4",
            22 => "S4",
            23 => "S5",
            24 => "S5",
            25 => "S6",
            26 => "S6",
            27 => "S7",
            28 => "S7",
            29 => "S8",
            30 => "S8",
            _ => "S9"  // 31 or above
        };
    }
}
