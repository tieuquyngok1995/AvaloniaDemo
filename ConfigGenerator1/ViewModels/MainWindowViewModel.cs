using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Threading;
using ReactiveUI;

namespace ConfigGenerator.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase _currentView;
        private readonly Dictionary<string, ViewModelBase> _views;

        public ViewModelBase CurrentView
        {
            get => _currentView;
            set => this.RaiseAndSetIfChanged(ref _currentView, value);
        }

        public ReactiveCommand<string, Unit> NavigateToCommand { get; }
        public ReactiveCommand<Unit, Unit> GenerateConfigCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveDraftCommand { get; }
        public ReactiveCommand<Unit, Unit> CancelCommand { get; }

        public MainWindowViewModel()
        {
            Debug.WriteLine("MainWindowViewModel constructor started");

            _views = new Dictionary<string, ViewModelBase>
            {
                { "sensorDataCollector", new SensorDataCollectorViewModel() },
                { "exchangeSync", new ExchangeSyncViewModel() },
                { "serviceManager", new ServiceManagerViewModel() }
            };

            // Mặc định hiển thị General Settings
            _currentView = _views["sensorDataCollector"];

            NavigateToCommand = ReactiveCommand.Create<string>(NavigateTo, outputScheduler: RxApp.MainThreadScheduler);
            GenerateConfigCommand = ReactiveCommand.Create(ExecuteGenerateConfig);
            SaveDraftCommand = ReactiveCommand.Create(ExecuteSaveDraft);
            CancelCommand = ReactiveCommand.Create(ExecuteCancel);

            Debug.WriteLine("MainWindowViewModel initialized successfully");
        }

        /// <summary>
        /// Navigate to the specified view
        /// </summary>
        /// <param name="viewName">Name of the view to navigate to</param>
        private void NavigateTo(string viewName)
        {
            Debug.WriteLine($"NavigateTo called with: {viewName}");
            Debug.WriteLine($"NavigateTo called with: {CurrentView}");
            try
            {
                Avalonia.Threading.Dispatcher.UIThread.Invoke(() =>
                {
                    if (_views.TryGetValue(viewName?.ToLowerInvariant() ?? string.Empty, out var viewModel))
                    {
                        CurrentView = viewModel;
                        Debug.WriteLine($"Changed CurrentView to {viewName}");
                    }
                    else
                    {
                        Debug.WriteLine($"View name '{viewName}' not found in dictionary");
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in NavigateTo: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }


        /// <summary>
        /// Generate configuration file based on current settings
        /// </summary>
        private void ExecuteGenerateConfig()
        {
            Debug.WriteLine("Generate Config button clicked");

            try
            {
                // This is already on UI thread because of ReactiveCommand.Create
                if (CurrentView is IConfigurable configurableViewModel)
                {
                    // For UI updates that need to happen immediately
                    configurableViewModel.GenerateConfig();

                    // For any heavy processing, schedule on a background thread
                    Task.Run(() =>
                    {
                        // Heavy processing here

                        // If you need to update UI after processing, dispatch back to UI thread:
                        Dispatcher.UIThread.Post(() =>
                        {
                            // Update UI with results
                        });
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in ExecuteGenerateConfig: {ex.Message}");
            }
        }

        /// <summary>
        /// Save current settings as draft
        /// </summary>
        private void ExecuteSaveDraft()
        {
            Debug.WriteLine("Save Draft button clicked");

            try
            {
                if (CurrentView is IConfigurable configurableViewModel)
                {
                    configurableViewModel.SaveDraft();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in ExecuteSaveDraft: {ex.Message}");
            }
        }

        /// <summary>
        /// Cancel current operation
        /// </summary>
        private void ExecuteCancel()
        {
            Debug.WriteLine("Cancel button clicked");

            try
            {
                if (CurrentView is IConfigurable configurableViewModel)
                {
                    configurableViewModel.Cancel();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in ExecuteCancel: {ex.Message}");
            }
        }

    }
}
