using ConfigGenerator.Interfaces;
using ConfigGenerator.Models;
using LanguageExt;

namespace ConfigGenerator.Utils;

/// <summary>
/// JSONファイルの読み書きを行うユーティリティクラス。
/// </summary>
public class FolderProcessor : IFolderProcessor
{
    public Either<AppError, bool> CreateFolder(string folderPath) =>
        FolderUtils.ValidateFolder(folderPath)
            .Bind(validPath => FolderUtils.CreateFolder(folderPath));
}
