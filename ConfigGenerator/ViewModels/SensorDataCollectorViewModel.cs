using System.Diagnostics;
using ReactiveUI;

namespace ConfigGenerator.ViewModels;

public partial class SensorDataCollectorViewModel : ViewModelBase, IConfigurableViewModel
{
    public string Greeting => "Welcome to SensorDataCollectorViewModel!";

    // Properties cho General Settings
    private string _appName;
    public string AppName
    {
        get => _appName;
        set => this.RaiseAndSetIfChanged(ref _appName, value);
    }

    private string _outputPath;
    public string OutputPath
    {
        get => _outputPath;
        set => this.RaiseAndSetIfChanged(ref _outputPath, value);
    }

    // Các phương thức yêu cầu bởi IConfigurableViewModel
    public void GenerateConfig()
    {
        // TODO: Gọi service để tạo file cấu hình
        Debug.WriteLine($"<---------- DEBUG: Generating general config for {AppName}");
    }

    public void SaveDraft()
    {
        Debug.WriteLine("<---------- DEBUG: Saving general settings draft");
    }

    public void Cancel()
    {
        // Reset giá trị về mặc định hoặc giá trị đã lưu trước đó
        AppName = string.Empty;
        OutputPath = string.Empty;
    }
}