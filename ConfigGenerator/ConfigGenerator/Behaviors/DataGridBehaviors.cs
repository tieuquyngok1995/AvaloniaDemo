using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;

namespace ConfigGenerator.Behaviors;

public static class DataGridBehaviors
{
    public static readonly AttachedProperty<object?> AutoScrollItemProperty =
         AvaloniaProperty.RegisterAttached<DataGrid, object?>(
             "AutoScrollItem",
             typeof(DataGridBehaviors)
         );

    public static void SetAutoScrollItem(DataGrid element, object? value) =>
        element.SetValue(AutoScrollItemProperty, value);

    public static object? GetAutoScrollItem(DataGrid element) =>
        element.GetValue(AutoScrollItemProperty);

    static DataGridBehaviors()
    {
        AutoScrollItemProperty.Changed.Subscribe(e =>
        {
            if (e.Sender is DataGrid dataGrid && e.NewValue.Value is object newItem)
            {
                Dispatcher.UIThread.Post(() =>
                {
                    dataGrid.ScrollIntoView(newItem, null);
                    dataGrid.SelectedItem = newItem;
                    dataGrid.Focus();
                }, DispatcherPriority.Background);
            }
        });
    }
}
