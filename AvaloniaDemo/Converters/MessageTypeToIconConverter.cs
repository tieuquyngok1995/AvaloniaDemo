using System;
using System.Globalization;
using Avalonia.Data.Converters;
using AvaloniaDemo.Interfaces;
using ConfigGenerator.Converters;

namespace AvaloniaDemo.Converters;

public class MessageTypeToIconConverter : IValueConverter
{
    public static readonly MessageTypeToIconConverter Instance = new();

    //public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //{
    //    if (value is MessageBoxType messageType)
    //    {
    //        return messageType switch
    //        {
    //            MessageBoxType.Information => "ℹ",
    //            MessageBoxType.Warning => "⚠",
    //            MessageBoxType.Error => "✕",
    //            MessageBoxType.Question => "?",
    //            _ => "ℹ"
    //        };
    //    }
    //    return "ℹ";
    //}
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is MessageBoxType type)
        {
            var key = type switch
            {
                MessageBoxType.Information => "LineHorizontal3Regular",
                MessageBoxType.Warning => "WarningIcon",
                MessageBoxType.Error => "ErrorIcon",
                MessageBoxType.Question => "QuestionIcon",
                _ => "LineHorizontal3Regular"
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
    public static readonly MessageTypeToColorConverter Instance = new();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is MessageBoxType messageType)
        {
            return messageType switch
            {
                MessageBoxType.Information => "DodgerBlue",
                MessageBoxType.Warning => "Orange",
                MessageBoxType.Error => "Red",
                MessageBoxType.Question => "Green",
                _ => "Gray"
            };
        }
        return "Gray";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
