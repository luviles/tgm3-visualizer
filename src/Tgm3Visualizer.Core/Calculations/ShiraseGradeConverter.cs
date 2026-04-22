namespace Tgm3Visualizer.Core.Calculations;

/// <summary>
/// Converts Shirase mode grade values to display names
/// Grade values: 0=Blank, 9=S1, 10=S2, ... 21=S13
/// </summary>
public static class ShiraseGradeConverter
{
    public static string ToGradeName(int grade)
    {
        return grade switch
        {
            0 => "--",
            9 => "S1",
            10 => "S2",
            11 => "S3",
            12 => "S4",
            13 => "S5",
            14 => "S6",
            15 => "S7",
            16 => "S8",
            17 => "S9",
            18 => "S10",
            19 => "S11",
            20 => "S12",
            >= 21 => "S13",
            _ => "--"
        };
    }
}
