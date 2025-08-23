using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls.ApplicationLifetimes;
using AvaloniaDemo.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaDemo.ViewModels;

public partial class CustomMessageBoxViewModel : ObservableObject
{
    //private Window _window;

    //public string Title { get; set; }
    //public string Message { get; set; }

    //public ICommand OkCommand { get; }
    //public ICommand CancelCommand { get; }

    //private TaskCompletionSource<bool> _tcs;

    //public CustomMessageBoxViewModel(Window window, string title, string message, TaskCompletionSource<bool> tcs)
    //{
    //    _window = window;
    //    Title = title;
    //    Message = message;
    //    _tcs = tcs;

    //    OkCommand = ReactiveCommand.Create(() => Close(true));
    //    CancelCommand = ReactiveCommand.Create(() => Close(false));
    //}
    //public void Close(bool result)
    //{
    //    if (!_tcs.Task.IsCompleted)   // đảm bảo chỉ gọi 1 lần
    //        _tcs.SetResult(result);

    //    _window.Close();              // đóng popup
    //}

    [ObservableProperty]
    private string _message = string.Empty;

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private MessageBoxType _messageType;

    [ObservableProperty]
    private bool _showYesNo;

    public MessageBoxResult Result { get; private set; } = MessageBoxResult.Cancel;
    public TaskCompletionSource<MessageBoxResult>? TaskCompletionSource { get; set; }

    [RelayCommand]
    public void OK()
    {
        Result = MessageBoxResult.OK;
        TaskCompletionSource?.SetResult(Result);
        // Đóng window
        if (TaskCompletionSource?.Task.IsCompleted == true)
        {
            CloseWindow();
        }
    }

    [RelayCommand]
    public void Cancel()
    {
        Result = MessageBoxResult.Cancel;
        TaskCompletionSource?.SetResult(Result);
        CloseWindow();
    }

    [RelayCommand]
    public void Yes()
    {
        Result = MessageBoxResult.Yes;
        TaskCompletionSource?.SetResult(Result);
        CloseWindow();
    }

    [RelayCommand]
    public void No()
    {
        Result = MessageBoxResult.No;
        TaskCompletionSource?.SetResult(Result);
        CloseWindow();
    }

    private void CloseWindow()
    {
        // Tìm window hiện tại và đóng nó
        if (Avalonia.Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var currentWindow = desktop.Windows.FirstOrDefault(w => w.DataContext == this);
            currentWindow?.Close();
        }
    }
}
