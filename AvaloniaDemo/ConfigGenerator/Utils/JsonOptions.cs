using System.Text.Encodings.Web;
using System.Text.Json;
using ConfigGenerator.Models;

namespace ConfigGenerator.Utils;

internal static class JsonOptions
{
    /// <summary>
    /// JSON シリアライザーの設定を定義します。インデント形式で書き込み、
    /// 型情報をエンコードするオプションを指定します。
    /// </summary>
    public static readonly JsonSerializerOptions optionsWriteIndentedEncoderTypeInfo = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        TypeInfoResolver = JsonSerializationContext.Default
    };

    /// <summary>
    /// プロパティ名の大文字・小文字を区別せず (PropertyNameCaseInsensitive = true)、
    /// TypeInfoResolver を使用してシリアライズ/デシリアライズを最適化するオプション。
    /// </summary>
    public static readonly JsonSerializerOptions optionsNameCaseTypeInfo = new()
    {
        PropertyNameCaseInsensitive = true,
        TypeInfoResolver = JsonSerializationContext.Default
    };
}
