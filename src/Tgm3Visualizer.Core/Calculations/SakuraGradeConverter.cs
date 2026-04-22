namespace Tgm3Visualizer.Core.Calculations;

/// <summary>
/// Converts Sakura mode grade values to display names
/// </summary>
public static class SakuraGradeConverter
{
    /// <summary>
    /// Convert a hi-score/grade value (1-based) to display name.
    /// Used for PlayerInfoCard hi-grade display.
    /// </summary>
    public static string ToGradeName(int grade)
    {
        return grade switch
        {
            >= 1 and <= 20 => grade.ToString(),
            21 => "EX1",
            22 => "EX2",
            23 => "EX3",
            24 => "EX4",
            25 => "EX5",
            26 => "EX6",
            27 => "EX7",
            >= 28 => "ALL",
            _ => "--"
        };
    }

    /// <summary>
    /// Convert a current stage level (0-based memory value) to display name.
    /// Memory stores 0-based index, so stage 0 = "1", stage 1 = "2", etc.
    /// </summary>
    public static string ToCurrentStageName(int zeroBasedStage)
    {
        if (zeroBasedStage < 0)
            return "1";
        return ToGradeName(zeroBasedStage + 1);
    }
}
