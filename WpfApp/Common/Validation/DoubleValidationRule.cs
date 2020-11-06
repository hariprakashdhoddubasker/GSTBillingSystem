using System.Windows.Controls;

namespace WpfApp.Common.Validation
{
    public class DoubleValidationRule : ValidationRule
    {
        public int Length { get; set; }
        public string Message { get; set; }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            var tempValue = value.ToString().Replace("₹","");
            if (double.TryParse(tempValue, out _) && tempValue.Length <= Length)
                return new ValidationResult(true, null);

            return new ValidationResult(false, Message);
        }
    }
}
