using System;
using System.IO;
using ConfigGenerator.Models;
using LanguageExt;
using static LanguageExt.Prelude;

namespace ConfigGenerator.Utils;

public static class FileUtils
{
    /// <summary>
    /// 指定されたファイルの妥当性を検証する。
    /// ファイルパスが null または空文字でないこと、かつファイルが存在することを確認する。
    /// 有効な場合は Right(filePath)、無効な場合は Left(FileError) を返す。
    /// </summary>
    /// <param name="filePath">検証するファイルのパス</param>
    /// <returns>成功時は Right(filePath)、失敗時は Left(FileError)</returns>
    public static Either<AppError, string> ValidateFile(string filePath) =>
        string.IsNullOrWhiteSpace(filePath)
            ? Left<AppError, string>(new AppError("無効なファイルパスです（null または空）", null, null))
            : !File.Exists(GetJSonFilePath(filePath))
                ? Left<AppError, string>(new AppError($"指定されたファイルが存在しません: {filePath}", null, null))
                : Right<AppError, string>(filePath);

    /// <summary>
    /// 指定されたファイルパスからテキストを読み込むメソッド。
    /// 読み込みに成功した場合は Right(ファイル内容)、
    /// 失敗した場合は Left(FileError) を返す。
    /// </summary>
    /// <param name="filePath">読み込む対象のファイルパス</param>
    /// <returns>成功時は Right(ファイル内容)、失敗時は Left(FileError)</returns>
    public static Either<AppError, string> ReadTextFromFile(string filePath) =>
        Try(() =>
        {
            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = new StreamReader(fs);
            return reader.ReadToEnd();
        })
        .ToEither(ex => new AppError($"ファイルの読み込みに失敗しました: {filePath}", ex.Message, ex.StackTrace));

    /// <summary>
    /// 指定されたファイルパスにテキストを書き込むメソッド。
    /// ファイルが存在しない場合は新規作成し、存在する場合は内容を上書きします。
    /// 書き込みに成功した場合は Right(true)、失敗した場合は Left(AppError) を返します。
    /// </summary>
    /// <param name="filePath">書き込み先のファイルパス</param>
    /// <param name="content">書き込むテキスト内容</param>
    /// <returns>成功時は Right(true)、失敗時は Left(AppError)</returns>
    public static Either<AppError, bool> WriteTextToFile(string filePath, string content) =>
        Try(() =>
        {
            using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            using var writer = new StreamWriter(fs);
            writer.Write(content);
            return true;
        })
        .ToEither(ex => new AppError($"ファイルの書き込みに失敗しました: {filePath}", ex.Message, ex.StackTrace));

    /// <summary>
    /// 指定されたファイルパスがルートパスかを確認し、
    /// ルートでない場合は基準ディレクトリを追加してフルパスを返します。
    /// </summary>
    /// <param name="filePath">確認する JSON ファイルのパス。</param>
    /// <returns>ルート付きのフルパス、存在しない場合は空の文字列。</returns>
    private static string GetJSonFilePath(string filePath) =>
         Path.IsPathRooted(filePath) && !filePath.StartsWith('/')
            ? filePath
            : Path.Combine(AppContext.BaseDirectory, filePath.TrimStart('/', '\\', '\u200B', '\uFEFF'));

}
