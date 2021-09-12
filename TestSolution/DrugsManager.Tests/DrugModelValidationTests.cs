using DrugsManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace DrugsManager.Tests
{
    public class DrugModelValidationTests : TestsBase
    {
        [Fact]
        public void DrugModel_ModelIsValid()
        {
            var result = new List<ValidationResult>();
            var validUser = DefaultDrugsList[0];
            
            var isValid = Validator.TryValidateObject(validUser, new ValidationContext(validUser), result, true);
            
            Assert.True(isValid);
        }

        [Fact]
        public void DrugModel_NdcIsShort()
        {
            var result = new List<ValidationResult>();
            var invalidUser = new Drug { Id = 111, Ndc = "1", Name = "First Drug", PackSize = 1, Unit = Unit.SmallPack, Price = 1.11m };

            var isValid = Validator.TryValidateObject(invalidUser, new ValidationContext(invalidUser), result, true);

            Assert.False(isValid);
            Assert.True(result.Count == 1);
            Assert.Equal("Ndc", result[0].MemberNames.ElementAt(0));
            Assert.Equal("The field Ndc must be a string with a minimum length of 8 and a maximum length of 8.", result[0].ErrorMessage);
        }

        [Fact]
        public void DrugModel_NdcIsLong()
        {
            var result = new List<ValidationResult>();
            var invalidUser = new Drug { Id = 111, Ndc = "1234567890", Name = "First Drug", PackSize = 1, Unit = Unit.SmallPack, Price = 1.11m };

            var isValid = Validator.TryValidateObject(invalidUser, new ValidationContext(invalidUser), result, true);

            Assert.False(isValid);
            Assert.True(result.Count == 1);
            Assert.Equal("Ndc", result[0].MemberNames.ElementAt(0));
            Assert.Equal("The field Ndc must be a string with a minimum length of 8 and a maximum length of 8.", result[0].ErrorMessage);
        }

        [Fact]
        public void DrugModel_NdcIsNotAlphaNumeric()
        {
            var result = new List<ValidationResult>();
            var invalidUser = new Drug { Id = 111, Ndc = "1234567@", Name = "First Drug", PackSize = 1, Unit = Unit.SmallPack, Price = 1.11m };

            var isValid = Validator.TryValidateObject(invalidUser, new ValidationContext(invalidUser), result, true);

            Assert.False(isValid);
            Assert.True(result.Count == 1);
            Assert.Equal("Ndc", result[0].MemberNames.ElementAt(0));
            Assert.Equal("The value must be alpha-numeric", result[0].ErrorMessage);
        }

        [Fact]
        public void DrugModel_NdcIsAbsent()
        {
            var result = new List<ValidationResult>();
            var invalidUser = new Drug { Id = 111, Name = "First Drug", PackSize = 1, Unit = Unit.SmallPack, Price = 1.11m };

            var isValid = Validator.TryValidateObject(invalidUser, new ValidationContext(invalidUser), result, true);

            Assert.False(isValid);
            Assert.True(result.Count == 1);
            Assert.Equal("Ndc", result[0].MemberNames.ElementAt(0));
            Assert.Equal("The Ndc field is required.", result[0].ErrorMessage);
        }

        [Fact]
        public void DrugModel_NameIsShort()
        {
            var result = new List<ValidationResult>();
            var invalidUser = new Drug { Id = 111, Ndc = "11111111", Name = "F", PackSize = 1, Unit = Unit.SmallPack, Price = 1.11m };

            var isValid = Validator.TryValidateObject(invalidUser, new ValidationContext(invalidUser), result, true);

            Assert.False(isValid);
            Assert.True(result.Count == 1);
            Assert.Equal("Name", result[0].MemberNames.ElementAt(0));
            Assert.Equal("The field Name must be a string with a minimum length of 3 and a maximum length of 255.", result[0].ErrorMessage);
        }

        [Fact]
        public void DrugModel_NameIsLong()
        {
            var result = new List<ValidationResult>();
            var invalidUser = new Drug { Id = 111, Ndc = "11111111", Name = GetString(256), PackSize = 1, Unit = Unit.SmallPack, Price = 1.11m };

            var isValid = Validator.TryValidateObject(invalidUser, new ValidationContext(invalidUser), result, true);

            Assert.False(isValid);
            Assert.True(result.Count == 1);
            Assert.Equal("Name", result[0].MemberNames.ElementAt(0));
            Assert.Equal("The field Name must be a string with a minimum length of 3 and a maximum length of 255.", result[0].ErrorMessage);
        }

        [Fact]
        public void DrugModel_NameIsAbsent()
        {
            var result = new List<ValidationResult>();
            var invalidUser = new Drug { Id = 111, Ndc = "11111111", PackSize = 1, Unit = Unit.SmallPack, Price = 1.11m };

            var isValid = Validator.TryValidateObject(invalidUser, new ValidationContext(invalidUser), result, true);

            Assert.False(isValid);
            Assert.True(result.Count == 1);
            Assert.Equal("Name", result[0].MemberNames.ElementAt(0));
            Assert.Equal("The Name field is required.", result[0].ErrorMessage);
        }

        [Fact]
        public void DrugModel_PackSizeIsSmall()
        {
            var result = new List<ValidationResult>();
            var invalidUser = new Drug { Id = 111, Ndc = "11111111", Name = "First Drug", PackSize = -5, Unit = Unit.SmallPack, Price = 1.11m };

            var isValid = Validator.TryValidateObject(invalidUser, new ValidationContext(invalidUser), result, true);

            Assert.False(isValid);
            Assert.True(result.Count == 1);
            Assert.Equal("PackSize", result[0].MemberNames.ElementAt(0));
            Assert.Equal("The field PackSize must be between 1 and 2147483647.", result[0].ErrorMessage);
        }

        [Fact]
        public void DrugModel_PackSizeIsAbsent()
        {
            var result = new List<ValidationResult>();
            var invalidUser = new Drug { Id = 111, Ndc = "11111111", Name = "First Drug", Unit = Unit.SmallPack, Price = 1.11m };

            var isValid = Validator.TryValidateObject(invalidUser, new ValidationContext(invalidUser), result, true);

            Assert.False(isValid);
            Assert.True(result.Count == 1);
            Assert.Equal("PackSize", result[0].MemberNames.ElementAt(0));
            Assert.Equal("The PackSize field is required.", result[0].ErrorMessage);
        }

        [Fact]
        public void DrugModel_UnitIsSmall()
        {
            var result = new List<ValidationResult>();
            var invalidUser = new Drug { Id = 111, Ndc = "11111111", Name = "First Drug", PackSize = 1, Unit = (Unit?)-5, Price = 1.11m };

            var isValid = Validator.TryValidateObject(invalidUser, new ValidationContext(invalidUser), result, true);

            Assert.False(isValid);
            Assert.True(result.Count == 1);
            Assert.Equal("Unit", result[0].MemberNames.ElementAt(0));
            Assert.Equal("The field Unit must be between 0 and 2.", result[0].ErrorMessage);
        }

        [Fact]
        public void DrugModel_UnitIsBig()
        {
            var result = new List<ValidationResult>();
            var invalidUser = new Drug { Id = 111, Ndc = "11111111", Name = "First Drug", PackSize = 1, Unit = (Unit?)5, Price = 1.11m };

            var isValid = Validator.TryValidateObject(invalidUser, new ValidationContext(invalidUser), result, true);

            Assert.False(isValid);
            Assert.True(result.Count == 1);
            Assert.Equal("Unit", result[0].MemberNames.ElementAt(0));
            Assert.Equal("The field Unit must be between 0 and 2.", result[0].ErrorMessage);
        }

        [Fact]
        public void DrugModel_UnitIsAbsent()
        {
            var result = new List<ValidationResult>();
            var invalidUser = new Drug { Id = 111, Ndc = "11111111", Name = "First Drug", PackSize = 1, Price = 1.11m };

            var isValid = Validator.TryValidateObject(invalidUser, new ValidationContext(invalidUser), result, true);

            Assert.False(isValid);
            Assert.True(result.Count == 1);
            Assert.Equal("Unit", result[0].MemberNames.ElementAt(0));
            Assert.Equal("The Unit field is required.", result[0].ErrorMessage);
        }

        [Fact]
        public void DrugModel_PriceTooMuchDigitsBeforeDot()
        {
            var result = new List<ValidationResult>();
            var invalidUser = new Drug { Id = 111, Ndc = "11111111", Name = "First Drug", PackSize = 1, Unit = Unit.SmallPack, Price = 11111111111.11m };

            var isValid = Validator.TryValidateObject(invalidUser, new ValidationContext(invalidUser), result, true);

            Assert.False(isValid);
            Assert.True(result.Count == 1);
            Assert.Equal("Price", result[0].MemberNames.ElementAt(0));
            Assert.Equal("Validation errors: Maximal number of digits before dot is 10;", result[0].ErrorMessage);
        }

        [Fact]
        public void DrugModel_PriceTooMuchDigitsAfterDot()
        {
            var result = new List<ValidationResult>();
            var invalidUser = new Drug { Id = 111, Ndc = "11111111", Name = "First Drug", PackSize = 1, Unit = Unit.SmallPack, Price = 1.111m };

            var isValid = Validator.TryValidateObject(invalidUser, new ValidationContext(invalidUser), result, true);

            Assert.False(isValid);
            Assert.True(result.Count == 1);
            Assert.Equal("Price", result[0].MemberNames.ElementAt(0));
            Assert.Equal("Validation errors: Maximal number of digits after dot is 2;", result[0].ErrorMessage);
        }

        [Fact]
        public void DrugModel_PriceTooLittleDigitsAfterDot()
        {
            var result = new List<ValidationResult>();
            var invalidUser = new Drug { Id = 111, Ndc = "11111111", Name = "First Drug", PackSize = 1, Unit = Unit.SmallPack, Price = 1.1m };

            var isValid = Validator.TryValidateObject(invalidUser, new ValidationContext(invalidUser), result, true);

            Assert.False(isValid);
            Assert.True(result.Count == 1);
            Assert.Equal("Price", result[0].MemberNames.ElementAt(0));
            Assert.Equal("Validation errors: Minimal number of digits after dot is 2;", result[0].ErrorMessage);
        }

        [Fact]
        public void DrugModel_PriceIsNegative()
        {
            var result = new List<ValidationResult>();
            var invalidUser = new Drug { Id = 111, Ndc = "11111111", Name = "First Drug", PackSize = 1, Unit = Unit.SmallPack, Price = -1.11m };

            var isValid = Validator.TryValidateObject(invalidUser, new ValidationContext(invalidUser), result, true);

            Assert.False(isValid);
            Assert.True(result.Count == 1);
            Assert.Equal("Price", result[0].MemberNames.ElementAt(0));
            Assert.Equal("Validation errors: Value must be non-negative;", result[0].ErrorMessage);
        }

        [Fact]
        public void DrugModel_PriceIsAbsent()
        {
            var result = new List<ValidationResult>();
            var invalidUser = new Drug { Id = 111, Ndc = "11111111", Name = "First Drug", PackSize = 1, Unit = Unit.SmallPack };

            var isValid = Validator.TryValidateObject(invalidUser, new ValidationContext(invalidUser), result, true);

            Assert.False(isValid);
            Assert.True(result.Count == 1);
            Assert.Equal("Price", result[0].MemberNames.ElementAt(0));
            Assert.Equal("The Price field is required.", result[0].ErrorMessage);
        }

        private string GetString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[new Random().Next(s.Length)]).ToArray());
        }
    }
}
