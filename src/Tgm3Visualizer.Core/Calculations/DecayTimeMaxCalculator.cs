namespace Tgm3Visualizer.Core.Calculations;

/// <summary>
/// Calculates the maximum decay time based on internal grade
/// </summary>
public static class DecayTimeMaxCalculator
{
    private static readonly int[] DecayRates = new int[]
    {
        125, // 0
        80,  // 1
        80,  // 2
        50,  // 3
        45,  // 4
        45,  // 5
        45,  // 6
        40,  // 7
        40,  // 8
        40,  // 9
        40,  // 10
        40,  // 11
        30,  // 12
        30,  // 13
        30,  // 14
        20,  // 15
        20,  // 16
        20,  // 17
        20,  // 18
        20,  // 19
        15,  // 20
        15,  // 21
        15,  // 22
        15,  // 23
        15,  // 24
        15,  // 25
        15,  // 26
        15,  // 27
        15,  // 28
        15,  // 29
        10,  // 30
        10   // 31+
    };

    /// <summary>
    /// Gets the maximum decay time for a given internal grade
    /// </summary>
    /// <param name="internalGrade">The internal grade (0-31+)</param>
    /// <returns>Maximum decay time value</returns>
    public static int GetMaxDecayTime(int internalGrade)
    {
        if (internalGrade < 0) return DecayRates[0];
        if (internalGrade >= DecayRates.Length) return DecayRates[^1]; // 31+ uses last value (10)
        return DecayRates[internalGrade];
    }
}
