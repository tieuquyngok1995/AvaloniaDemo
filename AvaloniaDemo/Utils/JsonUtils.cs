using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using ConfigGenerator.Models;
using LanguageExt;
using static LanguageExt.Prelude;

namespace ConfigGenerator.Utils;

public static class JsonUtils
{
    /// <summary>
    /// JSONC（コメント付き JSON）からコメントを削除し、JSON として処理可能な文字列を返す。
    /// </summary>
    /// <param name="jsonContent">コメント付き JSON 文字列</param>
    /// <returns>成功時は Right(JSON 文字列)、失敗時は Left(FileError)</returns>
    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "「JSON serialization」でエラーが発生しない")]
    public static Either<AppError, string> RemoveJsoncComments(string jsonContent) =>
        string.IsNullOrWhiteSpace(jsonContent)
        ? Left<AppError, string>(new AppError("JSON文字列が空または null です", null, null))
        : Try(() =>
        {
            using var doc = JsonDocument.Parse(jsonContent, new JsonDocumentOptions { CommentHandling = JsonCommentHandling.Skip });
            return JsonSerializer.Serialize(doc.RootElement, JsonOptions.optionsNameCaseTypeInfo);
        })
         .ToEither(ex => new AppError($"JSON文字列の変換処理中にエラーが発生しました", ex.Message, ex.StackTrace));

    /// <summary>
    /// JSON 文字列を指定された型にデシリアライズする。
    /// 成功時は Right(デシリアライズ結果)、失敗時は Left(FileError) を返す。
    /// デシリアライズが成功しても結果が null の場合はエラーとする。
    /// </summary>
    /// <typeparam name="T">デシリアライズ対象の型</typeparam>
    /// <param name="json">JSON 文字列</param>
    /// <returns>成功時は Right(T)、失敗時は Left(FileError)</returns>
    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "「JSON serialization」でエラーが発生しない")]
    public static Either<AppError, T> DeserializeJson<T>(string json) =>
        Try(() => JsonSerializer.Deserialize<T>(json, JsonOptions.optionsNameCaseTypeInfo))
            .ToEither(ex => new AppError("JSONデータのデシリアライズに失敗しました", ex.Message, ex.StackTrace))
            .Bind(data => data is not null
                ? Right<AppError, T>(data)
                : Left<AppError, T>(new AppError("デシリアライズは成功しましたが、結果のデータが null です", null, null)));

    /// <summary>
    /// JSON 配列、または JSON オブジェクト内の配列プロパティを List<T> にデシリアライズする。
    /// 成功時は Right(List<T>)、失敗時は Left(AppError) を返す。
    /// </summary>
    /// <typeparam name="T">デシリアライズ対象の型</typeparam>
    /// <param name="json">JSON 配列、または配列プロパティを含む JSON オブジェクト文字列</param>
    /// <param name="arrayPropertyName">配列を抽出するプロパティ名（既定値: AppConstants.DEFAULT_PROPERTY_NAME）</param>
    ///// <returns>成功時は Right(List<T>)、失敗時は Left(AppError)</returns>
    //[UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "「JSON serialization」でエラーが発生しない")]
    //public static Either<AppError, List<T>> DeserializeJsonList<T>(string json, string? arrayPropertyName = AppConstants.DEFAULT_PROPERTY_NAME) =>
    //    DeserializeJson<List<T>>(arrayPropertyName is not null ? UnwrapArrayProperty(json, arrayPropertyName) : json)
    //        .Bind(list => list is not null
    //            ? Right<AppError, List<T>>(list)
    //            : Left<AppError, List<T>>(new AppError("デシリアライズは成功しましたが、リストが null です", null, null)));

    /// <summary>
    /// JSON オブジェクトから、指定されたプロパティ名の配列を抽出し、
    /// 配列部分のみを JSON 文字列として返すユーティリティメソッド。
    /// プロパティが存在しない、または配列でない場合は例外をスローする。
    /// </summary>
    /// <param name="json">配列を含む JSON オブジェクト文字列</param>
    /// <param name="propertyName">抽出対象の配列プロパティ名</param>
    /// <returns>配列部分のみの JSON 文字列</returns>
    //public static string UnwrapArrayProperty(string json, string propertyName) =>
    //     json.TrimStart().StartsWith(AppConstants.JSON_ARRAY_START_CHAR) ? json :
    //        Try(() => JsonDocument.Parse(json)).Match(
    //        Succ: doc =>
    //            {
    //                using (doc)
    //                {
    //                    return doc.RootElement.ValueKind == JsonValueKind.Object &&
    //                           doc.RootElement.TryGetProperty(propertyName, out var arrProp) &&
    //                           arrProp.ValueKind == JsonValueKind.Array
    //                           ? arrProp.GetRawText() : json;
    //                }
    //            },
    //        Fail: _ => json);

    /// <summary>
    /// 指定されたオブジェクトを JSON 文字列にシリアライズします。
    /// 成功時は Right(JSON文字列)、失敗時は Left(AppError) を返します。
    /// </summary>
    /// <typeparam name="T">シリアライズする対象の型</typeparam>
    /// <param name="value">シリアライズするオブジェクト</param>
    /// <returns>成功時は Right(JSON文字列)、失敗時は Left(AppError)</returns>
    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "「JSON serialization」でエラーが発生しない")]
    public static Either<AppError, string> SerializeJson<T>(T value) =>
        value is null
            ? Left<AppError, string>(new AppError("シリアライズ対象の値が null です", null, null))
            : Try(() => JsonSerializer.Serialize(value, JsonOptions.optionsWriteIndentedEncoderTypeInfo))
                .ToEither(ex => new AppError("JSON文字列の変換処理中にエラーが発生しました", ex.Message, ex.StackTrace));
}
