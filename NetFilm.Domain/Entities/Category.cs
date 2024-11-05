namespace NetFilm.Domain.Entities
{
	public class Category : BaseEntity<Guid>
	{
        public string Name { get; set; }
		public ICollection<MovieCategory> MovieCategories { get; set; }
    }
}
