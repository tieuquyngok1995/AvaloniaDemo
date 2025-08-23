using System.Threading.Tasks;

namespace AvaloniaDemo.Interfaces
{
    public enum MessageBoxType
    {
        Information,
        Warning,
        Error,
        Question
    }

    public enum MessageBoxResult
    {
        OK,
        Cancel,
        Yes,
        No
    }

    public interface IMessageBoxService
    {
        //Task<bool> Show(string title, string message);

        Task<MessageBoxResult> ShowAsync(string message, string title = "Thông báo", MessageBoxType type = MessageBoxType.Information);
        Task<MessageBoxResult> ShowYesNoAsync(string message, string title = "Xác nhận");
        Task ShowInfoAsync(string message, string title = "Thông tin");
        Task ShowWarningAsync(string message, string title = "Cảnh báo");
        Task ShowErrorAsync(string message, string title = "Lỗi");
    }
}
