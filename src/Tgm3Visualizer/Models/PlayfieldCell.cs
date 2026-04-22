namespace Tgm3Visualizer.Models;

/// <summary>
/// Represents a single cell in the playfield grid
/// </summary>
public class PlayfieldCell
{
    /// <summary>
    /// Row index (0 = bottom row, 21 = top row)
    /// </summary>
    public int Row { get; set; }

    /// <summary>
    /// Column index (0 = left, 9 = right)
    /// </summary>
    public int Col { get; set; }

    /// <summary>
    /// Block type: 0=empty, 2=Red, 3=Green, 4=Pink, 5=Blue, 6=Orange, 7=Yellow, 8=Cyan
    /// </summary>
    public byte BlockType { get; set; }
}
