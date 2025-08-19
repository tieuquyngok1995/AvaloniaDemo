using System.Threading.Tasks;

namespace ConfigGenerator.Service;

/// <summary>
/// ファイル選択ダイアログを表示するためのサービスインターフェースです。
/// </summary>
public interface IFilePickerService
{
    /// <summary>
    /// ファイル選択ダイアログを表示し、選択されたファイルのパスを返します。
    /// </summary>
    /// <param name="title">ダイアログのタイトル（nullの場合はデフォルトタイトル）</param>
    /// <returns>選択されたファイルのパス。選択されなかった場合はnull。</returns>
    Task<string?> PickFilesAsync(string? title);
}
