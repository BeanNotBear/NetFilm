namespace NetFilm.Application.DTOs.SubtitleDTOs
{
	public class SubtitleDto
	{
        public Guid Id { get; set; }
        public string SubtitleName { get; set; }
		public string SubtitleUrl { get; set; }
		public Guid MovieId { get; set; }
	}
}
