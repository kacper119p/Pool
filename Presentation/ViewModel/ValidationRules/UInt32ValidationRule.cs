using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Presentation.ViewModel.ValidationRules
{
    internal class UInt32ValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is not string str) { return new ValidationResult(true, null); }
            if (!uint.TryParse(str, out uint _)) { return new ValidationResult(false, null); }
            return new ValidationResult(true, null);
        }
    }
}
