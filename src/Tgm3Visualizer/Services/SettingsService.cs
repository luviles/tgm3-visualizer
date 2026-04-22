using System.Text.Json;
using Windows.Storage;

namespace Tgm3Visualizer.Services;

public class SettingsService
{
    private const string SettingsFileName = "settings.json";
    private readonly ApplicationDataContainer _localSettings;

    public AppSettings Settings { get; private set; }

    public SettingsService()
    {
        _localSettings = ApplicationData.Current.LocalSettings;
        Settings = LoadSettings();
    }

    private AppSettings LoadSettings()
    {
        if (_localSettings.Values.TryGetValue(SettingsFileName, out var json))
        {
            try
            {
                return JsonSerializer.Deserialize<AppSettings>(json?.ToString() ?? "") ?? new AppSettings();
            }
            catch
            {
                return new AppSettings();
            }
        }
        return new AppSettings();
    }

    public void SaveSettings()
    {
        var json = JsonSerializer.Serialize(Settings);
        _localSettings.Values[SettingsFileName] = json;
    }
}

public class AppSettings
{
    public bool AlwaysOnTop { get; set; }
    public int WindowWidth { get; set; } = 400;
    public int WindowHeight { get; set; } = 600;
    public string BackgroundColor { get; set; } = "#1A1A2E";
    public int UpdateIntervalMs { get; set; } = 16;
}
