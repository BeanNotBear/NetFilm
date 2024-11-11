using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NetFilm.Application.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class PhoneNumberAttribute : ValidationAttribute
    {
        private const string PhoneNumberPattern = @"^(0[3|5|7|8|9])+([0-9]{8})$";

        public PhoneNumberAttribute()
        {
            ErrorMessage = "Invalid Vietnamese phone number format.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string phoneNumber)
            {
                if (!Regex.IsMatch(phoneNumber, PhoneNumberPattern))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            else if (value != null)
            {
                return new ValidationResult("Invalid phone number type.");
            }

            return ValidationResult.Success;
        }
    }
}
