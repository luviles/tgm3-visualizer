using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Tgm3Visualizer.Controls.Common;

public sealed partial class AtomicReverseProgressBar : UserControl
{
    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register(nameof(Value), typeof(double), typeof(AtomicReverseProgressBar),
                                    new PropertyMetadata(0.0, OnValueChanged));

    public double Maximum
    {
        get => (double)GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    public static readonly DependencyProperty MaximumProperty =
        DependencyProperty.Register(nameof(Maximum), typeof(double), typeof(AtomicReverseProgressBar),
                                    new PropertyMetadata(1.0, OnMaximumChanged));

    public double ProgressWidth
    {
        get => (double)GetValue(ProgressWidthProperty);
        set => SetValue(ProgressWidthProperty, value);
    }

    public static readonly DependencyProperty ProgressWidthProperty =
        DependencyProperty.Register(nameof(ProgressWidth), typeof(double), typeof(AtomicReverseProgressBar),
                                    new PropertyMetadata(0.0));

    public AtomicReverseProgressBar()
    {
        InitializeComponent();
        SizeChanged += OnSizeChanged;
    }

    private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ((AtomicReverseProgressBar)d).UpdateProgressWidth();
    }

    private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ((AtomicReverseProgressBar)d).UpdateProgressWidth();
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        UpdateProgressWidth();
    }

    private void UpdateProgressWidth()
    {
        if (Maximum > 0 && ActualWidth > 0)
            ProgressWidth = Math.Clamp(ActualWidth * (Value / Maximum), 0, ActualWidth);
        else
            ProgressWidth = 0;
    }
}
