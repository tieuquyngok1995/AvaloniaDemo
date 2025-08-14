using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConfigGenerator.Models;
using ConfigGenerator.Service;

namespace ConfigGenerator.ViewModels;

public partial class ExchangeSyncSettingsViewModel : ViewModelBase
{
    public ObservableCollection<ExchangeResourceMappingModel> DataTable { get; } = [];

    public ObservableCollection<ComboBoxModel> ExchangeConnectionSources { get; } = AppData.ExchangeConnectionSources;

    public ObservableCollection<ComboBoxModel> DataOutputModes { get; } = AppData.DataOutputModes;

    [ObservableProperty]
    private ExchangeResourceMappingModel? _selectedRow;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsSignageVisible))]
    [NotifyPropertyChangedFor(nameof(IsManagementSiteVisible))]
    [NotifyPropertyChangedFor(nameof(IsCustomVisible))]
    private ComboBoxModel? _selectedExchangeConnectionSource;


    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsJsonFileVisible))]
    [NotifyPropertyChangedFor(nameof(IsJsonServerVisible))]
    private ComboBoxModel? _selectedDataOutputMode;

    [RelayCommand]
    private void AddRow()
    {
        var newRow = new ExchangeResourceMappingModel(DataTable.Count + 1, "", "");
        DataTable.Add(newRow);

        SelectedRow = newRow;
    }

    [RelayCommand]
    private void RemoveSelected(ExchangeResourceMappingModel? selected)
    {
        if (selected != null)
            DataTable.Remove(selected);
        DataTable.Select((item, index) => new { item, index })
         .ToList()
         .ForEach(x => x.item.No = x.index + 1);
    }
    private readonly IFilePickerService _filePickerService;

    [ObservableProperty]
    private string? selectedFiles;

    public ExchangeSyncSettingsViewModel(IFilePickerService filePickerService)
    {
        _filePickerService = filePickerService;
    }

    public ExchangeSyncSettingsViewModel()
    : this(new DesignTimeFilePickerService())
    {
    }

    // Service giả cho chế độ design-time
    private class DesignTimeFilePickerService : IFilePickerService
    {
        public Task<string?> PickFilesAsync(string? title = null)
            => Task.FromResult<string?>("sample.txt");
    }

    [RelayCommand]
    private async Task SelectFilesAsync()
    {
        SelectedFiles = await _filePickerService.PickFilesAsync("Hello from Avalonia");
    }


    public bool IsSignageVisible => SelectedExchangeConnectionSource?.Key == 0;
    public bool IsManagementSiteVisible => SelectedExchangeConnectionSource?.Key == 1;
    public bool IsCustomVisible => SelectedExchangeConnectionSource?.Key == 2;



    public bool IsJsonFileVisible => SelectedDataOutputMode?.Key == 0;
    public bool IsJsonServerVisible => SelectedDataOutputMode?.Key == 1;

}
