namespace ConfigGenerator.Models;

/// <summary>
/// コンボボックスの項目を表すモデルクラスです。
/// Key（数値）とValue（文字列）を持ち、
/// DisplayTextプロパティで「Key: Value」の形式で表示できます。
/// </summary>
public record ComboBoxModel(int Key, string Value)
{
    /// <summary>
    /// 「Key: Value」の形式で表示するためのプロパティです。
    /// </summary>
    public string DisplayText => $"{Key}: {Value}";
}
