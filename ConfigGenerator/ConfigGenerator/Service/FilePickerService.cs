using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace ConfigGenerator.Service;

internal class FilePickerService : IFilePickerService
{
    [System.Obsolete]
    public async Task<string?> PickFilesAsync(string? title)
    {
        var dialog = new OpenFileDialog
        {
            Title = title ?? "Chọn file",
            AllowMultiple = false
        };

        var window = GetMainWindow();
        if (window is null)
            return null;

        var result = await dialog.ShowAsync(window);

        // Nếu có file được chọn, trả về file đầu tiên
        return result?.FirstOrDefault();
    }

    private Window? GetMainWindow()
    {
        // Lấy main window của ứng dụng
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            return desktop.MainWindow;
        }

        return null;
    }
}
