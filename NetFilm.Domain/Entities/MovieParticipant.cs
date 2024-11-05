namespace NetFilm.Domain.Entities
{
	public class MovieParticipant
	{
		public Guid MovieId { get; set; }
		public Guid ParticipantId { get; set; }
		public Movie Movie { get; set; }
		public Participant Participant { get; set; }
	}
}
