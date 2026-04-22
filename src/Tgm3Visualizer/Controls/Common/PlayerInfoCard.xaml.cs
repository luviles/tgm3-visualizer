using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Tgm3Visualizer.Controls.Common;

public sealed partial class PlayerInfoCard : UserControl
{
    public string Nickname
    {
        get => (string)GetValue(NicknameProperty);
        set => SetValue(NicknameProperty, value);
    }

    public string VisibleNickname
    {
        get => (string)GetValue(VisibleNicknameProperty);
        set => SetValue(VisibleNicknameProperty, value);
    }

    public string DecorationPoints
    {
        get => (string)GetValue(DecorationPointsProperty);
        set => SetValue(DecorationPointsProperty, value);
    }

    public bool IsLoggedIn
    {
        get => (bool)GetValue(IsLoggedInProperty);
        set => SetValue(IsLoggedInProperty, value);
    }

    public string WorldEasyHiScore
    {
        get => (string)GetValue(WorldEasyHiScoreProperty);
        set => SetValue(WorldEasyHiScoreProperty, value);
    }

    public string ClassicEasyHiScore
    {
        get => (string)GetValue(ClassicEasyHiScoreProperty);
        set => SetValue(ClassicEasyHiScoreProperty, value);
    }

    public string WorldMasterGrade
    {
        get => (string)GetValue(WorldMasterGradeProperty);
        set => SetValue(WorldMasterGradeProperty, value);
    }

    public string ClassicMasterGrade
    {
        get => (string)GetValue(ClassicMasterGradeProperty);
        set => SetValue(ClassicMasterGradeProperty, value);
    }

    public string WorldSakuraGrade
    {
        get => (string)GetValue(WorldSakuraGradeProperty);
        set => SetValue(WorldSakuraGradeProperty, value);
    }

    public string ClassicSakuraGrade
    {
        get => (string)GetValue(ClassicSakuraGradeProperty);
        set => SetValue(ClassicSakuraGradeProperty, value);
    }

    public string WorldShiraseGrade
    {
        get => (string)GetValue(WorldShiraseGradeProperty);
        set => SetValue(WorldShiraseGradeProperty, value);
    }

    public string ClassicShiraseGrade
    {
        get => (string)GetValue(ClassicShiraseGradeProperty);
        set => SetValue(ClassicShiraseGradeProperty, value);
    }

    public static readonly DependencyProperty NicknameProperty =
        DependencyProperty.Register(nameof(Nickname), typeof(string), typeof(PlayerInfoCard), new PropertyMetadata("", OnDisplayedInfoChanged));

    public static readonly DependencyProperty VisibleNicknameProperty =
        DependencyProperty.Register(nameof(VisibleNickname), typeof(string), typeof(PlayerInfoCard), new PropertyMetadata("TGM3 Visualizer"));

    public static readonly DependencyProperty DecorationPointsProperty =
        DependencyProperty.Register(nameof(DecorationPoints), typeof(string), typeof(PlayerInfoCard), new PropertyMetadata(""));

    public static readonly DependencyProperty IsLoggedInProperty =
        DependencyProperty.Register(nameof(IsLoggedIn), typeof(bool), typeof(PlayerInfoCard), new PropertyMetadata(false, OnDisplayedInfoChanged));

    public static readonly DependencyProperty WorldEasyHiScoreProperty =
        DependencyProperty.Register(nameof(WorldEasyHiScore), typeof(string), typeof(PlayerInfoCard), new PropertyMetadata("--"));

    public static readonly DependencyProperty ClassicEasyHiScoreProperty =
        DependencyProperty.Register(nameof(ClassicEasyHiScore), typeof(string), typeof(PlayerInfoCard), new PropertyMetadata("--"));

    public static readonly DependencyProperty WorldMasterGradeProperty =
        DependencyProperty.Register(nameof(WorldMasterGrade), typeof(string), typeof(PlayerInfoCard), new PropertyMetadata("--"));

    public static readonly DependencyProperty ClassicMasterGradeProperty =
        DependencyProperty.Register(nameof(ClassicMasterGrade), typeof(string), typeof(PlayerInfoCard), new PropertyMetadata("--"));

    public static readonly DependencyProperty WorldSakuraGradeProperty =
        DependencyProperty.Register(nameof(WorldSakuraGrade), typeof(string), typeof(PlayerInfoCard), new PropertyMetadata("--"));

    public static readonly DependencyProperty ClassicSakuraGradeProperty =
        DependencyProperty.Register(nameof(ClassicSakuraGrade), typeof(string), typeof(PlayerInfoCard), new PropertyMetadata("--"));

    public static readonly DependencyProperty WorldShiraseGradeProperty =
        DependencyProperty.Register(nameof(WorldShiraseGrade), typeof(string), typeof(PlayerInfoCard), new PropertyMetadata("--"));

    public static readonly DependencyProperty ClassicShiraseGradeProperty =
        DependencyProperty.Register(nameof(ClassicShiraseGrade), typeof(string), typeof(PlayerInfoCard), new PropertyMetadata("--"));

    public PlayerInfoCard()
    {
        InitializeComponent();
    }

    private static void OnDisplayedInfoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var card = (PlayerInfoCard)d;
        card.VisibleNickname = !string.IsNullOrWhiteSpace(card.Nickname)
            ? card.Nickname
            : "TGM3 Visualizer";
    }
}
