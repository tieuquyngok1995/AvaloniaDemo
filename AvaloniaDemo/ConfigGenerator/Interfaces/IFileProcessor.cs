using ConfigGenerator.Models;
using LanguageExt;

namespace ConfigGenerator.Interfaces;

public interface IFileProcessor
{
    public Either<AppError, bool> CreateFile(string filePath);

    /// <summary>
    /// 指定されたパスの JSON ファイルから任意の型のデータを読み込み、デシリアライズします。
    /// </summary>
    public Either<AppError, T> ReadFromJsonFile<T>(string filePath);

    /// <summary>
    /// 指定された RoomsModel のデータを、指定されたパスの JSON ファイルにシリアライズして書き込みます。
    /// </summary>
    public Either<AppError, bool> WriteToJsonFile(SensorDataCollectorSettingsModel roomsModel, string filePath);
}
