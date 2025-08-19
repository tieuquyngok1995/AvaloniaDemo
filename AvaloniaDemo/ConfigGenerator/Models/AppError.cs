using System;

namespace ConfigGenerator.Models;

public record AppError(string Message, string? ExceptionMessage, string? StackTrace)
{
    /// <summary>
    /// 詳細なエラーメッセージを取得する
    /// </summary>
    /// <returns>フォーマットされたエラーメッセージ（メッセージ、例外メッセージ、スタックトレースを含む）</returns>
    public string GetError() =>
        $"{Message}。{(ExceptionMessage != null ? $"\n例外メッセージ: {ExceptionMessage}" : "")}" +
        $"{(StackTrace != null ? $"\nスタックトレース:{StackTrace[2..]}" : "")}";

    /// <summary>
    /// Exception オブジェクトに変換する
    /// </summary>
    /// <returns>FileError 情報を含む Exception</returns>
    public Exception ToException() =>
        new InvalidOperationException(Message)
        {
            Data = { ["ExceptionMessage"] = ExceptionMessage, ["StackTrace"] = StackTrace }
        };
}
