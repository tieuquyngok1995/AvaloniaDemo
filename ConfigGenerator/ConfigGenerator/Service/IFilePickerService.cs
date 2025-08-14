using System.Threading.Tasks;

namespace ConfigGenerator.Service;

public interface IFilePickerService
{
    Task<string?> PickFilesAsync(string? title);
}
