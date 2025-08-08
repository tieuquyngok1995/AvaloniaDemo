using System.Diagnostics;
using Avalonia.Controls;
using ConfigGenerator.ViewModels;

namespace ConfigGenerator.Desktop.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        Debug.WriteLine("<---------- DEBUG: Initializing MainWindow");
        InitializeComponent();
        DataContext = new MainWindowViewModel();
        Debug.WriteLine("<---------- DEBUG: MainWindow initialized with DataContext");
    }
}
