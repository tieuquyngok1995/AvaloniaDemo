using ConfigGenerator.ViewModels;
using FluentAvalonia.UI.Windowing;

namespace ConfigGenerator.Views;

public partial class MainWindow : AppWindow
{
    public MainWindow(MainViewModel vm)
    {
        DataContext = vm;
        InitializeComponent();

        SplashScreen = new ComplexSplashScreen();

        TitleBar.ExtendsContentIntoTitleBar = true;
        TitleBar.TitleBarHitTestType = TitleBarHitTestType.Complex;
    }

    public MainWindow() : this(new MainViewModel()) { }
}
