using System;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using ConfigGenerator.ViewModels;

namespace ConfigGenerator.Desktop
{
    /// <summary>
    /// Locates views for ViewModels
    /// </summary>
    public class ViewLocator : IDataTemplate
    {
        /// <summary>
        /// Builds the appropriate view for the given data
        /// </summary>
        public Control Build(object data)
        {
            if (data is null)
                return new TextBlock { Text = "No data" };

            var name = data.GetType().FullName!.Replace("ViewModel", "View");
            var type = Type.GetType(name);

            Debug.WriteLine($"<----------- ViewLocator looking for view: {name}");

            if (type != null)
            {
                Debug.WriteLine($"<----------- ViewLocator found view: {name}");
                return (Control)Activator.CreateInstance(type)!;
            }

            // For temporary DummyViewModel
            if (data is MainViewModel dummyVm)
            {
                Debug.WriteLine($"<----------- MainViewModel found view: {data}");
                // return new TextBlock { Text = dummyVm.Title };
            }

            Debug.WriteLine($"<----------- ViewLocator could not find view for {data.GetType().FullName}");
            return new TextBlock { Text = $"View not found: {data.GetType().FullName}" };
        }

        /// <summary>
        /// Determines if this template should be used for the given data
        /// </summary>
        public bool Match(object data)
        {
            return data is ViewModelBase;
        }
    }
}
