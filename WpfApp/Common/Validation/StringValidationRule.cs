using System;
using System.Windows.Controls;

namespace WpfApp.Common.Validation
{
    public class StringValidationRule : ValidationRule
    {
        public int Length { get; set; }
        public string Message { get; set; }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(value)) && value.ToString().Length <= Length)
                return new ValidationResult(true, null);

            return new ValidationResult(false, Message);
        }
    }
}
