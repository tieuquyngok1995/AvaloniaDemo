namespace ConfigGenerator.Models;

using CommunityToolkit.Mvvm.ComponentModel;

/// <summary>
/// Exchangeリソースのマッピング情報を保持するモデルクラスです。
/// No（番号）、Name（名称）、ExchangeResource（Exchangeリソース名）を持ちます。
/// </summary>
public partial class ExchangeResourceMappingModel(int no, string name, string exchangeResource) : ObservableObject
{
    /// <summary>
    /// 項目番号
    /// </summary>
    [ObservableProperty]
    private int _no = no;

    /// <summary>
    /// 項目名
    /// </summary>
    [ObservableProperty]
    private string _name = name;

    /// <summary>
    /// Exchangeリソース名
    /// </summary>
    [ObservableProperty]
    private string _exchangeResource = exchangeResource;
}
