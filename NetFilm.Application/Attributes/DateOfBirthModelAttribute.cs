using System.ComponentModel.DataAnnotations;

namespace NetFilm.Application.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DateOfBirthModelAttribute : ValidationAttribute
    {
        public DateOfBirthModelAttribute()
        {
            ErrorMessage = "Date of birth must be a past or present date.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateOnly date)
            {
                if (date > DateOnly.FromDateTime(DateTime.Now))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
