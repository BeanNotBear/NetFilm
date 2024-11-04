using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Domain.Entities
{
	/// <summary>
	/// This is base entity
	/// </summary>
	/// <typeparam name="TId">Data type of entity</typeparam>
	public class BaseEntity<TId>
	{
		public TId Id { get; set; }
		public DateTime CreatedAt { get; set; }
		public TId CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public TId UpdatedBy { get; set; }
    }
}
