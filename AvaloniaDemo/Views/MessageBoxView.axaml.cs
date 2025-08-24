using Avalonia.Controls;
using AvaloniaDemo.Interfaces;
using AvaloniaDemo.ViewModels;

namespace AvaloniaDemo.Views;

public partial class MessageBoxView : Window
{
    public MessageBoxView()
    {
        InitializeComponent();
    }


    public MessageBoxView(MessageBoxViewModel viewModel) : this()
    {
        DataContext = viewModel;

        // Subscribe to result changes để đóng window
        viewModel.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(MessageBoxViewModel.Result)
                && viewModel.TaskCompletionSource != null)
            {
                Close(viewModel.Result);
            }
        };
    }

    protected override void OnClosed(System.EventArgs e)
    {
        if (DataContext is MessageBoxViewModel vm && vm.TaskCompletionSource?.Task.IsCompleted == false)
        {
            vm.TaskCompletionSource.SetResult(MessageBoxResult.Cancel);
        }
        base.OnClosed(e);
    }
}
