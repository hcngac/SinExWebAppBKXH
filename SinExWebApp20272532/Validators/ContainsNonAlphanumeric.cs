using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace SinExWebApp20272532.Validators
{
    public class ContainsNonAlphanumeric : ValidationAttribute
    {
        private int min;
        public ContainsNonAlphanumeric(int minimumNumber) : base("The {0} field must contain at least 2 non-alphanumerical character.")
        {
            min = minimumNumber;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int countOfNonAlphanumeric = 0;
            if (value != null)
            {
                var valueAsString = value.ToString();
                foreach (char i in valueAsString)
                {
                    if (!("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".Contains(i)))
                    {
                        ++countOfNonAlphanumeric;
                        if (countOfNonAlphanumeric >= min)
                            return ValidationResult.Success;
                    }
                }
                var errorMsg = FormatErrorMessage(validationContext.DisplayName);
                return new ValidationResult(errorMsg);
            }
            return ValidationResult.Success;
        }
    }
}