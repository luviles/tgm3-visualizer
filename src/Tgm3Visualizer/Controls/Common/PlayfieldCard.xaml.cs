using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Tgm3Visualizer.Assets;

namespace Tgm3Visualizer.Controls.Common;

public sealed partial class PlayfieldCard : UserControl
{
    private WriteableBitmap? _playfieldBitmap;
    private string _currentControlMode = "CLASSIC";

    private const int PlayfieldPixelWidth = 10 * BlockPixelData.BlockSize;  // 160
    private const int PlayfieldPixelHeight = 20 * BlockPixelData.BlockSize; // 320

    public byte[,] PlayfieldData
    {
        get => (byte[,])GetValue(PlayfieldDataProperty);
        set => SetValue(PlayfieldDataProperty, value);
    }

    public static readonly DependencyProperty PlayfieldDataProperty =
        DependencyProperty.Register(nameof(PlayfieldData), typeof(byte[,]), typeof(PlayfieldCard),
                                    new PropertyMetadata(null, OnPlayfieldDataChanged));

    public string ControlMode
    {
        get => (string)GetValue(ControlModeProperty);
        set => SetValue(ControlModeProperty, value);
    }

    public static readonly DependencyProperty ControlModeProperty =
        DependencyProperty.Register(nameof(ControlMode), typeof(string), typeof(PlayfieldCard),
                                    new PropertyMetadata("CLASSIC", OnControlModeChanged));

    public WriteableBitmap? PlayfieldBitmap
    {
        get => _playfieldBitmap;
        set
        {
            _playfieldBitmap = value;
            SetValue(PlayfieldBitmapProperty, value);
        }
    }

    public static readonly DependencyProperty PlayfieldBitmapProperty =
        DependencyProperty.Register(nameof(PlayfieldBitmap), typeof(WriteableBitmap), typeof(PlayfieldCard),
                                    new PropertyMetadata(null));

    public int LockDelay
    {
        get => (int)GetValue(LockDelayProperty);
        set => SetValue(LockDelayProperty, value);
    }

    public static readonly DependencyProperty LockDelayProperty =
        DependencyProperty.Register(nameof(LockDelay), typeof(int), typeof(PlayfieldCard),
                                    new PropertyMetadata(0, OnLockInfoChanged));

    public int MaxLockFrame
    {
        get => (int)GetValue(MaxLockFrameProperty);
        set => SetValue(MaxLockFrameProperty, value);
    }

    public static readonly DependencyProperty MaxLockFrameProperty =
        DependencyProperty.Register(nameof(MaxLockFrame), typeof(int), typeof(PlayfieldCard),
                                    new PropertyMetadata(30, OnLockInfoChanged));

    public string LockDelayDisplayText
    {
        get => (string)GetValue(LockDelayDisplayTextProperty);
        set => SetValue(LockDelayDisplayTextProperty, value);
    }

    public static readonly DependencyProperty LockDelayDisplayTextProperty =
        DependencyProperty.Register(nameof(LockDelayDisplayText), typeof(string), typeof(PlayfieldCard),
                                    new PropertyMetadata("Lock Delay: 0/30F"));

    private static void OnLockInfoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var card = (PlayfieldCard)d;
        card.LockDelayDisplayText = $"Lock Delay: {card.LockDelay}/{card.MaxLockFrame}F";
    }

    public string MoveResetText
    {
        get => (string)GetValue(MoveResetTextProperty);
        set => SetValue(MoveResetTextProperty, value);
    }

    public static readonly DependencyProperty MoveResetTextProperty =
        DependencyProperty.Register(nameof(MoveResetText), typeof(string), typeof(PlayfieldCard),
                                    new PropertyMetadata(""));

    private static void OnPlayfieldDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var card = (PlayfieldCard)d;
        card.UpdatePlayfieldBitmap((byte[,])e.NewValue);
    }

    private static void OnControlModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var card = (PlayfieldCard)d;
        card._currentControlMode = (string)e.NewValue;
        if (card.PlayfieldData != null)
            card.UpdatePlayfieldBitmap(card.PlayfieldData);
    }

    public PlayfieldCard()
    {
        InitializeComponent();
        InitializePlayfieldBitmap();
    }

    private void InitializePlayfieldBitmap()
    {
        PlayfieldBitmap = new WriteableBitmap(PlayfieldPixelWidth, PlayfieldPixelHeight);
    }

    private void UpdatePlayfieldBitmap(byte[,]? playfield)
    {
        if (PlayfieldBitmap == null || playfield == null) return;

        using (var stream = PlayfieldBitmap.PixelBuffer.AsStream())
        {
            for (int displayY = 0; displayY < 20; displayY++)
            {
                int gameY = 19 - displayY;
                for (int x = 0; x < 10; x++)
                {
                    byte blockType = playfield[gameY, x];
                    var pixelData = BlockPixelData.GetBlockData(blockType, _currentControlMode) ?? BlockPixelData.Empty;

                    for (int py = 0; py < BlockPixelData.BlockSize; py++)
                    {
                        int destOffset = ((displayY * BlockPixelData.BlockSize + py) * PlayfieldPixelWidth + x * BlockPixelData.BlockSize) * 4;
                        int srcOffset = py * BlockPixelData.BlockSize * 4;

                        stream.Position = destOffset;
                        stream.Write(pixelData, srcOffset, BlockPixelData.BlockSize * 4);
                    }
                }
            }
        }

        PlayfieldBitmap.Invalidate();
    }
}
