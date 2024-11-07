namespace NetFilm.Application.DTOs.MovieDTOs
{
	public class StreamingResult
	{
		public Stream Stream { get; set; }
		public string ContentType { get; set; }
		public long ContentLength { get; set; }
		public long Start { get; set; }
		public long End { get; set; }
		public long FileSize { get; set; }
	}
}
