using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Domain.Entities
{
	public class Country : BaseEntity<Guid>
	{
        public string Name { get; set; }
    }
}
