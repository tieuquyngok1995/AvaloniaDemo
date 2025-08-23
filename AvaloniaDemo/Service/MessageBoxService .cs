using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using AvaloniaDemo.Interfaces;
using AvaloniaDemo.ViewModels;
using AvaloniaDemo.Views;

namespace AvaloniaDemo.Service;

public class MessageBoxService : IMessageBoxService
{
    //public async Task<bool> Show(string title, string message)
    //{
    //    var mainWindow = (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
    //    if (mainWindow == null)
    //        return false;

    //    var tcs = new TaskCompletionSource<bool>();
    //    var msgBox = new AvaloniaDemo.CustomMessageBox();
    //    msgBox.DataContext = new AvaloniaDemo.ViewModels.CustomMessageBoxViewModel(msgBox, title, message, tcs);

    //    await msgBox.ShowDialog(mainWindow); // modal

    //    return await tcs.Task;
    //}

    public async Task<MessageBoxResult> ShowAsync(string message, string title = "Thông báo", MessageBoxType type = MessageBoxType.Information)
    {
        return await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync((System.Func<Task<MessageBoxResult>>)(async () =>
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

    public async Task<MessageBoxResult> ShowYesNoAsync(string message, string title = "Xác nhận")
    {
        return await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync((System.Func<Task<MessageBoxResult>>)(async () =>
        {
            var viewModel = new MessageBoxViewModel
            {
                Message = message,
                Title = title,
                MessageType = MessageBoxType.Question,
                ShowYesNo = true
            };

            return await ShowDialog(viewModel);
        }));
    }

    public async Task ShowInfoAsync(string message, string title = "Thông tin")
    {
        await ShowAsync(message, title, MessageBoxType.Information);
    }

    public async Task ShowWarningAsync(string message, string title = "Cảnh báo")
    {
        await ShowAsync(message, title, MessageBoxType.Warning);
    }

    public async Task ShowErrorAsync(string message, string title = "Lỗi")
    {
        await ShowAsync(message, title, MessageBoxType.Error);
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
