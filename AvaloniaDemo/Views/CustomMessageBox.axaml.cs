using Avalonia.Controls;
using AvaloniaDemo.Interfaces;
using AvaloniaDemo.ViewModels;

namespace AvaloniaDemo;

public partial class CustomMessageBox : Window
{
    public CustomMessageBox()
    {
        InitializeComponent();
    }

    //public CustomMessageBox(CustomMessageBoxViewModel viewModel) : this()
    //{
    //    DataContext = viewModel;
    //}

    //private void Close_Click(object sender, RoutedEventArgs e)
    //{
    //    var vm = DataContext as CustomMessageBoxViewModel;
    //    vm?.Close(false);  // trả false khi đóng bằng X
    //}

    public CustomMessageBox(CustomMessageBoxViewModel viewModel) : this()
    {
        DataContext = viewModel;

        // Subscribe to result changes để đóng window
        viewModel.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(CustomMessageBoxViewModel.Result)
                && viewModel.TaskCompletionSource != null)
            {
                Close(viewModel.Result);
            }
        };
    }

    protected override void OnClosed(System.EventArgs e)
    {
        if (DataContext is CustomMessageBoxViewModel vm && vm.TaskCompletionSource?.Task.IsCompleted == false)
        {
            vm.TaskCompletionSource.SetResult(MessageBoxResult.Cancel);
        }
        base.OnClosed(e);
    }
}
