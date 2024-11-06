using System.ComponentModel.DataAnnotations;

namespace NetFilm.Application.DTOs.BucketDTOs
{
	public class AddBucketRequestDto
	{
		[Required]
		public string BucketName { get; set; }
	}
}
