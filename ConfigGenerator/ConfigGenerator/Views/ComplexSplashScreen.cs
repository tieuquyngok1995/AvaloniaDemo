using System.Threading;
using System.Threading.Tasks;
using Avalonia.Media;
using FluentAvalonia.UI.Windowing;

namespace ConfigGenerator.Views;

internal class ComplexSplashScreen : IApplicationSplashScreen
{
    public string AppName => "";

    public IImage? AppIcon => null;

    public object SplashScreenContent { get; }

    public int MinimumShowTime => 0;

    public ComplexSplashScreen()
    {
        SplashScreenContent = new SplashScreenView();
    }

    public async Task RunTasks(CancellationToken cancellationToken)
    {
        await ((SplashScreenView)SplashScreenContent).InitApp();
    }
}
