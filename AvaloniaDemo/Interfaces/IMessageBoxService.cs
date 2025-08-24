using System.Threading.Tasks;

namespace AvaloniaDemo.Interfaces
{
    public enum MessageBoxType
    {
        Information,
        Warning,
        Error,
        Confirmation,
        Processing
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
        Task<MessageBoxResult> ShowAsync(string message, string title = "Notification", MessageBoxType type = MessageBoxType.Information);
        Task<MessageBoxResult> ShowYesNoAsync(string message, string title = "Confirmation");
        Task ShowInfoAsync(string message, string title = "Information");
        Task ShowWarningAsync(string message, string title = "Warning");
        Task ShowErrorAsync(string message, string title = "Error");
        Task<MessageBoxResult> ShowProcessingAsync(string message);
    }
}
