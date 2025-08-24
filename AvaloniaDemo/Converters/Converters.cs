using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using AvaloniaDemo.Interfaces;
using ConfigGenerator.Converters;

namespace AvaloniaDemo.Converters;

public static class Converters
{
    public static readonly MessageTypeToIconConverter MessageTypeToIcon = new();
    public static readonly MessageTypeToColorConverter MessageTypeToColor = new();
}

public class MessageTypeToIconConverter : IValueConverter
{
    public static readonly MessageTypeToIconConverter Instance = new();
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is MessageBoxType type)
        {
            var key = type switch
            {
                MessageBoxType.Information => "InfoIcon",
                MessageBoxType.Warning => "WarningIcon",
                MessageBoxType.Error => "ErrorIcon",
                MessageBoxType.Confirmation => "QuestionIcon",
                MessageBoxType.Processing => "ProcessingIcon",
                _ => null
            };
            return TypeConverters.IconConverter.Convert(key, targetType, parameter, culture);
        }

        return TypeConverters.IconConverter.Convert(null, targetType, parameter, culture);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class MessageTypeToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is MessageBoxType messageType)
        {
            return messageType switch
            {
                MessageBoxType.Information => Brushes.DodgerBlue,
                MessageBoxType.Warning => Brushes.SandyBrown,
                MessageBoxType.Error => Brushes.Red,
                MessageBoxType.Confirmation => Brushes.DeepSkyBlue,
                _ => Brushes.Gray
            };
        }

        return Brushes.Gray;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
