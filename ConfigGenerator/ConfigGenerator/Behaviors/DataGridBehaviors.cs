using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;

namespace ConfigGenerator.Behaviors;

public static class DataGridBehaviors
{
    /// <summary>
    /// DataGridの指定したアイテムに自動スクロールし、選択状態・フォーカスを設定するための添付プロパティです。
    /// </summary>
    public static readonly AttachedProperty<object?> AutoScrollItemProperty =
         AvaloniaProperty.RegisterAttached<DataGrid, object?>(
             "AutoScrollItem",
             typeof(DataGridBehaviors)
         );

    /// <summary>
    /// AutoScrollItemプロパティを設定します。
    /// </summary>
    /// <param name="element">対象のDataGrid</param>
    /// <param name="value">スクロール・選択・フォーカスするアイテム</param>
    public static void SetAutoScrollItem(DataGrid element, object? value) =>
        element.SetValue(AutoScrollItemProperty, value);

    /// <summary>
    /// AutoScrollItemプロパティの値を取得します。
    /// </summary>
    /// <param name="element">対象のDataGrid</param>
    /// <returns>現在設定されているアイテム</returns>
    public static object? GetAutoScrollItem(DataGrid element) =>
        element.GetValue(AutoScrollItemProperty);

    static DataGridBehaviors()
    {
        // AutoScrollItemプロパティが変更されたときの処理
        AutoScrollItemProperty.Changed.Subscribe(e =>
        {
            if (e.Sender is DataGrid dataGrid && e.NewValue.Value is object newItem)
            {
                // UIスレッドでスクロール・選択・フォーカスを実行
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
