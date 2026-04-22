namespace Tgm3Visualizer.Core.Calculations;

/// <summary>
/// Converts grade numeric values to display names
/// </summary>
public static class GradeConverter
{
    /// <summary>
    /// Grade value to name mapping for TGM3 Master mode
    /// TODO: Verify exact mapping with actual game values
    /// </summary>
    private static readonly string[] GradeNames = new string[]
    {
        "9", "8", "7", "6", "5", "4", "3", "2", "1",  // 0-8
        "S1", "S2", "S3", "S4", "S5", "S6", "S7", "S8", "S9",  // 9-17
        "m1", "m2", "m3", "m4", "m5", "m6", "m7", "m8", "m9",  // 18-26
        "M",   // 27
        "MK",  // 28 - Master K
        "MV",  // 29 - Master V
        "MO",  // 30 - Master O
        "MM",  // 31 - Master M
        "GM",  // 32 - Grand Master if available!!
    };

    /// <summary>
    /// Convert grade numeric value to display name
    /// </summary>
    /// <param name="gradeValue">Grade value from memory</param>
    /// <returns>Grade display name (e.g., "GM", "M", "S1")</returns>
    public static string ToGradeName(int gradeValue)
    {
        if (gradeValue < 0)
        {
            return "--";
        }
        else if(gradeValue >= GradeNames.Length)
        {
            return "GM";
        }

        return GradeNames[gradeValue];
    }
}
