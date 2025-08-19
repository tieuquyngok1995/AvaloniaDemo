using System.IO;
using ConfigGenerator.Models;
using LanguageExt;
using static LanguageExt.Prelude;

namespace ConfigGenerator.Utils;

public static class FolderUtils
{
    public static Either<AppError, string> ValidateFolder(string folderPath) =>
        string.IsNullOrWhiteSpace(folderPath)
            ? Left<AppError, string>(new AppError("無効なフォルダパスです（null または空）", null, null))
            : !Directory.Exists(folderPath)
                ? Right<AppError, string>(folderPath)
                : Left<AppError, string>(new AppError($"指定されたフォルダが存在しません: {folderPath}", null, null));


    public static Either<AppError, bool> CreateFolder(string folderPath) =>
        string.IsNullOrWhiteSpace(folderPath)
            ? Left<AppError, bool>(new AppError("無効なフォルダパスです（null または空）", null, null))
            : Right<AppError, bool>(Directory.CreateDirectory(folderPath) != null);

}
