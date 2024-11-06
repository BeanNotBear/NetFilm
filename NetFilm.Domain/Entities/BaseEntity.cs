using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
		[Key]
		public TId Id { get; set; }

        public bool IsDelete { get; set; }
    }
}
