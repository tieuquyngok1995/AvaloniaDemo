namespace ConfigGenerator.Models;

using CommunityToolkit.Mvvm.ComponentModel;

public partial class ExchangeResourceMappingModel(int no, string name, string exchangeResource) : ObservableObject
{
    [ObservableProperty]
    private int _no = no;

    [ObservableProperty]
    private string _name = name;

    [ObservableProperty]
    private string _exchangeResource = exchangeResource;
}
