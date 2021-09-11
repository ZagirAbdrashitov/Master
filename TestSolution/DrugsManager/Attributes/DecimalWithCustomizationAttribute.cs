using System;
using System.ComponentModel.DataAnnotations;

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
            var result = true;
            ErrorMessage = "Validation errors:";
            var beforeDot = GetNumberOfDigitsBeforeDot((decimal)value);
            var afterDot = GetNumberOfDigitsAfterDot((decimal)value);
            if (beforeDot < _minDigitsBeforeDot)
            {
                ErrorMessage = $"{ErrorMessage} Minimal number of digits before dot is {_minDigitsBeforeDot};";
                result = false;
            }
            if (beforeDot > _maxDigitsBeforeDot)
            {
                ErrorMessage = $"{ErrorMessage} Maximal number of digits before dot is {_maxDigitsBeforeDot};";
                result = false;
            }
            if (afterDot < _minDigitsAfterDot)
            {
                ErrorMessage = $"{ErrorMessage} Minimal number of digits after dot is {_minDigitsAfterDot};";
                result = false;
            }
            if (afterDot > _maxDigitsAfterDot)
            {
                ErrorMessage = $"{ErrorMessage} Maximal number of digits before dot is {_maxDigitsAfterDot};";
                result = false;
            }
            if ((decimal)value < 0 && !_allowNegative)
            {
                ErrorMessage = $"{ErrorMessage} Value must be non-negative;";
                result = false;
            }

            return result;
        }

        private int GetNumberOfDigitsBeforeDot(decimal value)
        {
            return Math.Truncate(Math.Abs(value)).ToString().Length;
        }

        private int GetNumberOfDigitsAfterDot(decimal value)
        {
            return BitConverter.GetBytes(decimal.GetBits(value)[3])[2];
        }
    }
}
