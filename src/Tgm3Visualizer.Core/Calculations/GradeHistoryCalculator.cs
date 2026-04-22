using System;
using System.Collections.Generic;
using System.Linq;

namespace Tgm3Visualizer.Core.Calculations;

/// <summary>
/// Result of grade history calculation for Master mode
/// </summary>
public class GradeHistoryResult
{
    public List<GradeHistoryEntry> Entries { get; set; } = new();
    public string AverageGrade { get; set; } = "N/A";
}

public class GradeHistoryEntry
{
    public string GradeName { get; set; } = "";
    public bool IsHighlighted { get; set; }
}

/// <summary>
/// Calculates Master mode grade history display:
/// - Excludes the single highest grade (oldest for ties)
/// - Selects top 3 from remaining (newest for ties)
/// - Computes average of selected 3
/// </summary>
public static class GradeHistoryCalculator
{
    /// <summary>
    /// Format grade points with smart decimal handling:
    /// - +0.12 → "+0.12" (keep both decimals)
    /// - +0.10 → "+0.1" (remove trailing zero)
    /// - +1.00 → "+1" (remove decimal if whole number)
    /// </summary>
    public static string FormatGradePoints(double value)
    {
        double rounded = Math.Round(value, 2);
        if (rounded == Math.Floor(rounded))
            return $"+{(int)rounded}";
        string formatted = rounded.ToString("0.##");
        return $"+{formatted}";
    }

    public static GradeHistoryResult Calculate(int[] masterGradeHistory)
    {
        var result = new GradeHistoryResult();

        // Memory values are +1 compared to actual grade values
        // Memory 0 = invalid (-1), Memory 1+ = valid (grade 0+ = "9", "8", ...)
        var allHistoryGrades = masterGradeHistory
            .Reverse()
            .Select((g, index) => new { MemoryValue = g, Grade = g - 1, Index = index })
            .ToList();

        var validGrades = allHistoryGrades
            .Where(x => x.MemoryValue > 0)
            .ToList();

        if (validGrades.Count == 0)
        {
            result.Entries.Add(new GradeHistoryEntry
            {
                GradeName = "No History",
                IsHighlighted = false
            });
            return result;
        }

        // Step 1: Find top 1 to exclude (highest grade, oldest/leftmost for ties)
        var excluded = validGrades
            .OrderByDescending(x => x.Grade)
            .ThenBy(x => x.Index)
            .First();

        var remaining = validGrades.Where(x => x.Index != excluded.Index).ToList();

        // Step 2: Pick top 3 from remaining (highest grade, newest/rightmost for ties)
        var top234 = remaining
            .OrderByDescending(x => x.Grade)
            .ThenByDescending(x => x.Index)
            .Take(3)
            .ToList();

        var top234Indices = top234.Select(x => x.Index).ToHashSet();

        foreach (var item in validGrades)
        {
            result.Entries.Add(new GradeHistoryEntry
            {
                GradeName = GradeConverter.ToGradeName(item.Grade),
                IsHighlighted = top234Indices.Contains(item.Index)
            });
        }

        // Adjusted Average Grade (need at least 4 valid grades: 1 excluded + 3 selected)
        if (top234.Count == 3)
        {
            var avgGrade = (int)Math.Floor(top234.Average(x => x.Grade));
            result.AverageGrade = GradeConverter.ToGradeName(avgGrade);
        }

        return result;
    }
}
