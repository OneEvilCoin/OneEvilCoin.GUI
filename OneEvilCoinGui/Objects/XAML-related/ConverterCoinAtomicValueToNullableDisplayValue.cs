﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace OneEvil.OneEvilCoinGUI
{
    public class ConverterCoinAtomicValueToNullableDisplayValue : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Properties.Resources.PunctuationQuestionMark;
            return (ulong)value / StaticObjects.CoinAtomicValueDivider;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is double)) return null;
            return (ulong)Math.Round((double)value * StaticObjects.CoinAtomicValueDivider);
        }
    }
}
