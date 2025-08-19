using ConfigGenerator.Models;
using LanguageExt;

namespace ConfigGenerator.Interfaces;

public interface IFolderProcessor
{
    public Either<AppError, bool> CreateFolder(string folderPath);
}
