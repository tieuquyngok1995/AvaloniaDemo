using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using AvaloniaDemo.Interfaces;
using AvaloniaDemo.ViewModels;
using AvaloniaDemo.Views;

namespace AvaloniaDemo.Service;

public class MessageBoxService : IMessageBoxService
{

    public async Task<MessageBoxResult> ShowAsync(string message, string title = "Notification", MessageBoxType type = MessageBoxType.Information)
    {
        return await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync((async () =>
        {
            var viewModel = new MessageBoxViewModel
            {
                Message = message,
                Title = title,
                MessageType = type,
                ShowYesNo = false
            };

            return await ShowDialog(viewModel);
        }));
    }

    public async Task<MessageBoxResult> ShowYesNoAsync(string message, string title = "Confirmation")
    {
        return await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync((System.Func<Task<MessageBoxResult>>)(async () =>
        {
            var viewModel = new MessageBoxViewModel
            {
                Message = message,
                Title = title,
                MessageType = MessageBoxType.Confirmation,
                ShowYesNo = true
            };

            return await ShowDialog(viewModel);
        }));
    }

    public async Task ShowInfoAsync(string message, string title = "Information")
    {
        await ShowAsync(message, title, MessageBoxType.Information);
    }

    public async Task ShowWarningAsync(string message, string title = "Warning")
    {
        await ShowAsync(message, title, MessageBoxType.Warning);
    }

    public async Task ShowErrorAsync(string message, string title = "Error")
    {
        await ShowAsync(message, title, MessageBoxType.Error);
    }

    public async Task<MessageBoxResult> ShowProcessingAsync(string message)
    {
        return await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync((System.Func<Task<MessageBoxResult>>)(async () =>
        {
            var viewModel = new MessageBoxViewModel
            {
                Message = message,
                Title = "Processing",
                MessageType = MessageBoxType.Processing,
                ShowProcessing = true
            };

            return await ShowDialog(viewModel);
        }));

    }


    private async Task<MessageBoxResult> ShowDialog(MessageBoxViewModel viewModel)
    {
        // luôn tạo TaskCompletionSource
        var tcs = new TaskCompletionSource<MessageBoxResult>();
        viewModel.TaskCompletionSource = tcs;

        var window = new MessageBoxView(viewModel);

        // Tìm main window
        Window? mainWindow = null;
        if (Avalonia.Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            mainWindow = desktop.MainWindow;
        }

        // Hiển thị dialog
        if (mainWindow != null)
        {
            // dùng ShowDialog -> kết quả từ OnClosed/PropertyChanged sẽ set vào TaskCompletionSource
            window.ShowDialog(mainWindow);
        }
        else
        {
            // fallback không có mainWindow
            window.Show();
        }

        return await tcs.Task; // luôn chờ TaskCompletionSource
    }

}
