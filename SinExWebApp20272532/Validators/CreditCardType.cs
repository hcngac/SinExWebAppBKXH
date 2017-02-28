using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SinExWebApp20272532.Validators
{
    public class CreditCardType : ValidationAttribute
    {
        private readonly string[] CreditCardTypes = new string[] { "American Express", "Diners Club", "Discover", "MasterCard", "UnionPay", "Visa" };
        public CreditCardType():base("The {0} field must match one of the credit card types.")
        {
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var valueAsString = value.ToString();
                foreach(string i in CreditCardTypes)
                {
                    if (valueAsString == i)
                    {
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