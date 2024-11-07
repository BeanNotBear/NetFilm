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
    public class PasswordAttribute : ValidationAttribute
    {
        private const int MinLength = 8;

        public PasswordAttribute()
        {
            ErrorMessage = $"Password must be at least {MinLength} characters long, and include uppercase, lowercase, digit, and special character.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string password)
            {
                // Check minimum length
                if (password.Length < MinLength)
                {
                    return new ValidationResult(ErrorMessage);
                }

                // Check for at least one uppercase letter
                if (!Regex.IsMatch(password, @"[A-Z]"))
                {
                    return new ValidationResult(ErrorMessage);
                }

                // Check for at least one lowercase letter
                if (!Regex.IsMatch(password, @"[a-z]"))
                {
                    return new ValidationResult(ErrorMessage);
                }

                // Check for at least one digit
                if (!Regex.IsMatch(password, @"[0-9]"))
                {
                    return new ValidationResult(ErrorMessage);
                }

                // Check for at least one special character
                if (!Regex.IsMatch(password, @"[\W_]"))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            else if (value != null)
            {
                return new ValidationResult("Invalid password type.");
            }

            return ValidationResult.Success;
        }
    }
}
