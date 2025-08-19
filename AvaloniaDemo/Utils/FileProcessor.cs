using System.Diagnostics.CodeAnalysis;
using ConfigGenerator.Interfaces;
using ConfigGenerator.Models;
using LanguageExt;

namespace ConfigGenerator.Utils;

/// <summary>
/// JSONファイルの読み書きを行うユーティリティクラス。
/// </summary>
public class FileProcessor : IFileProcessor
{
    public Either<AppError, bool> CreateFile(string filePath) =>
    FileUtils.ValidateFile(filePath)
        .Bind(validPath => FileUtils.WriteTextToFile(validPath, string.Empty));

    /// <summary>
    /// 指定されたパスの JSON ファイルから任意の型のデータを読み込み、デシリアライズします。
    /// </summary>
    /// <typeparam name="T">デシリアライズ対象の型</typeparam>
    /// <param name="filePath">JSONファイルのパス</param>
    /// <returns>読み込み成功時は Right(T)、失敗時は Left(AppError)</returns>
    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "「JSON serialization」でエラーが発生しない")]
    public Either<AppError, T> ReadFromJsonFile<T>(string filePath) =>
        FileUtils.ValidateFile(filePath)
            .Bind(validPath => FileUtils.ReadTextFromFile(validPath))
            .Bind(jsonContent => JsonUtils.DeserializeJson<T>(jsonContent));

    /// <summary>
    /// 指定された RoomsModel のデータを、指定されたパスの JSON ファイルにシリアライズして書き込みます。
    /// </summary>
    /// <param name="roomsModel">書き込む RoomsModel のインスタンス</param>
    /// <param name="filePath">JSONファイルのパス</param>
    /// <returns>書き込み成功時は Right(true)、失敗時は Left(AppError)</returns>
    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "「JSON serialization」でエラーが発生しない")]
    public Either<AppError, bool> WriteToJsonFile(SensorDataCollectorSettingsModel roomsModel, string filePath) =>
        FileUtils.ValidateFile(filePath)
        .Bind(_ => JsonUtils.SerializeJson(roomsModel))
        .Bind(jsonContent => FileUtils.WriteTextToFile(filePath, jsonContent));
}
