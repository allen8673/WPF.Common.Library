using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace WPF.Common.Library.Model
{
    public class Uri2StringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Uri input = value as Uri;
            return input == null ? String.Empty : $"{input.ToString().TrimEnd('/')}/";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string input = value as string;
            return String.IsNullOrEmpty(input) ? null : new Uri($"{input.TrimEnd('/')}/", UriKind.Absolute);
        }
    }

    public class UriValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string input = value as string;
            if (String.IsNullOrEmpty(input)) // Valid input, converts to null.
                return new ValidationResult(true, null);

            Uri outUri;
            return Uri.TryCreate(input, UriKind.Absolute, out outUri) ?
                new ValidationResult(true, null) :
                new ValidationResult(false, "String is not a valid URI");
        }
    }
}
