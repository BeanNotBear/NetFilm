using Microsoft.AspNetCore.Identity;
namespace NetFilm.Domain.Entities
{
	public class User : IdentityUser<Guid>
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime DateOfBirth { get; set; }
	}
}
