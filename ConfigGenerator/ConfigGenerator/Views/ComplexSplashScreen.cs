using System.Threading;
using System.Threading.Tasks;
using Avalonia.Media;
using FluentAvalonia.UI.Windowing;

namespace ConfigGenerator.Views;

/// <summary>
/// 複雑なスプラッシュスクリーンを提供するクラスです。
/// アプリ名やアイコン、スプラッシュ画面の内容、初期化タスクなどを管理します。
/// </summary>
internal class ComplexSplashScreen : IApplicationSplashScreen
{
    /// <summary>
    /// アプリケーション名（未使用の場合は空文字列）
    /// </summary>
    public string AppName => string.Empty;

    /// <summary>
    /// アプリケーションアイコン（未使用の場合はnull）
    /// </summary>
    public IImage? AppIcon => null;

    /// <summary>
    /// スプラッシュ画面に表示する内容
    /// </summary>
    public object SplashScreenContent { get; }

    /// <summary>
    /// スプラッシュ画面の最小表示時間（ミリ秒）
    /// </summary>
    public int MinimumShowTime => 0;

    /// <summary>
    /// コンストラクタ。スプラッシュ画面の内容を初期化します。
    /// </summary>
    public ComplexSplashScreen()
    {
        SplashScreenContent = new SplashScreenView();
    }

    /// <summary>
    /// スプラッシュ画面表示中に実行する初期化タスクです。
    /// </summary>
    /// <param name="cancellationToken">キャンセルトークン</param>
    public async Task RunTasks(CancellationToken cancellationToken)
    {
        await ((SplashScreenView)SplashScreenContent).InitApp();
    }
}
