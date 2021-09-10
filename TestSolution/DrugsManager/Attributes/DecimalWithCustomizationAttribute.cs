using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DrugsManager.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    sealed public class DecimalWithCustomizationAttribute : ValidationAttribute
    {
        private readonly uint _maxDigitsBeforeDot;
        private readonly uint _minDigitsBeforeDot;
        private readonly uint _maxDigitsAfterDot;
        private readonly uint _minDigitsAfterDot;
        private readonly bool _allowNegative;

        public DecimalWithCustomizationAttribute(uint minDigitsBeforeDot, uint maxDigitsBeforeDot, uint minDigitsAfterDot, uint maxDigitsAfterDot, bool allowNegative)
        {
            _maxDigitsBeforeDot = maxDigitsBeforeDot;
            _minDigitsBeforeDot = minDigitsBeforeDot;
            _maxDigitsAfterDot = maxDigitsAfterDot;
            _minDigitsAfterDot = minDigitsAfterDot;
            _allowNegative = allowNegative;
        }

        public override bool IsValid(object value)
        {
            var pattern = @$"^{(_allowNegative ? "[-]?" : string.Empty)}\d{{{_minDigitsBeforeDot},{_maxDigitsBeforeDot}}}(\.|\,)\d{{{_minDigitsAfterDot},{_maxDigitsAfterDot}}}$";

            return Regex.IsMatch(value.ToString(), pattern);
        }
    }
}
