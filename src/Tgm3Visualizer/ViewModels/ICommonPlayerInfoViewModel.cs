namespace Tgm3Visualizer.ViewModels;

/// <summary>
/// Interface for ViewModels that display common player info (PlayerInfoCard).
/// </summary>
public interface ICommonPlayerInfoViewModel
{
    string Nickname { get; set; }
    string DecorationPoints { get; set; }
    bool IsLoggedIn { get; set; }
    string WorldEasyHiScore { get; set; }
    string ClassicEasyHiScore { get; set; }
    string WorldMasterGrade { get; set; }
    string ClassicMasterGrade { get; set; }
    string WorldSakuraGrade { get; set; }
    string ClassicSakuraGrade { get; set; }
    string WorldShiraseGrade { get; set; }
    string ClassicShiraseGrade { get; set; }
}
