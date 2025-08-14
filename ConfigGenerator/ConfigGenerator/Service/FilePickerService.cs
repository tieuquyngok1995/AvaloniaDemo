using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace ConfigGenerator.Service;

internal class FilePickerService : IFilePickerService
{
    /// <summary>
    /// ファイル選択ダイアログを表示し、選択されたファイルのパスを返します。
    /// </summary>
    /// <param name="title">ダイアログのタイトル（nullの場合はデフォルトタイトル）</param>
    /// <returns>選択されたファイルのパス。選択されなかった場合はnull。</returns>
    [System.Obsolete]
    public async Task<string?> PickFilesAsync(string? title)
    {
        var dialog = new OpenFileDialog
        {
            Title = title ?? "ファイルを選択",
            AllowMultiple = false
        };

        Window? window = GetMainWindow();
        if (window is null)
        {
            return null;
        }

        var result = await dialog.ShowAsync(window);

        return result?.FirstOrDefault();
    }

    /// <summary>
    /// アプリケーションのメインウィンドウを取得します。
    /// </summary>
    /// <returns>メインウィンドウ。取得できない場合はnull。</returns>
    private static Window? GetMainWindow()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            return desktop.MainWindow;
        }

        return null;
    }
}
