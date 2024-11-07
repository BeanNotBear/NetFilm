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
    public class CustomEmailAttribute : ValidationAttribute
    {
        private const string EmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        public CustomEmailAttribute()
        {
            ErrorMessage = "Invalid email format.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string email)
            {
                if (!Regex.IsMatch(email, EmailPattern))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            else if (value != null)
            {
                return new ValidationResult("Invalid email type.");
            }

            return ValidationResult.Success;
        }
    }
}
