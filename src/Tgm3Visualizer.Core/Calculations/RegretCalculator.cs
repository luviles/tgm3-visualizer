using Tgm3Visualizer.Core.Models;

namespace Tgm3Visualizer.Core.Calculations;

/// <summary>
/// Calculates Regret status for Master mode sections
/// </summary>
public static class RegretCalculator
{
    /// <summary>
    /// Check if a Regret was triggered for a section
    /// </summary>
    /// <param name="sectionTimeFrames">Time taken for the section in frames</param>
    /// <param name="section">Section index (0-9)</param>
    /// <returns>True if Regret conditions are met</returns>
    public static bool IsRegret(int sectionTimeFrames, int section)
    {
        if (section >= SectionConstants.RegretTimes.Length)
            return false;

        var threshold = SectionConstants.GetRegretThresholdFrames(section);
        return sectionTimeFrames > threshold;
    }

    /// <summary>
    /// Check if a Regret was triggered for a Shirase section
    /// </summary>
    /// <param name="sectionTimeFrames">Time taken for the section in frames</param>
    /// <param name="section">Section index (0-12)</param>
    /// <returns>True if Regret conditions are met</returns>
    public static bool IsShiraseRegret(int sectionTimeFrames, int section)
    {
        if (section >= SectionConstants.ShiraseRegretTimes.Length)
            return false;

        var threshold = SectionConstants.GetShiraseRegretThresholdFrames(section);
        return sectionTimeFrames > threshold;
    }

}
