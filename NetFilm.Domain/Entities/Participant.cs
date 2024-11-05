using NetFilm.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Domain.Entities
{
	public class Participant : BaseEntity<Guid>
	{
        [Column(TypeName = "nvarchar")]
		[MaxLength(100)]
		public string Name { get; set; }

		[Column(TypeName = "int")]
		[Range(0, 2)]
		public RoleInMovie RoleInMovie { get; set; }
        public ICollection<MovieParticipant> MovieParticipants { get; set; }
    }
}
