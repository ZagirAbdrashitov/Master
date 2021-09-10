using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DrugsManager.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    sealed public class AlphaNumeric : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var pattern = "^[a-zA-Z0-9]*$";

            return Regex.IsMatch(value.ToString(), pattern);
        }
    }
}
