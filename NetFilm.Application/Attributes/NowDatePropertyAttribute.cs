using System.ComponentModel.DataAnnotations;

namespace NetFilm.Application.Attributes
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class NowDatePropertyAttribute : ValidationAttribute
	{
		public NowDatePropertyAttribute()
		{
			ErrorMessage = "Date must be a future or present date.";
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (value is DateOnly date)
			{
				if (date < DateOnly.FromDateTime(DateTime.Now))
				{
					return new ValidationResult(ErrorMessage);
				}
			}

			return ValidationResult.Success;
		}
	}
}
