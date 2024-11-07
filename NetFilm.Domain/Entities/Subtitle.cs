using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Domain.Entities
{
    public class Subtitle : BaseEntity<Guid>
    {
        public string SubtitleName { get; set; }
        public string SubtitleUrl { get; set; }
        public Guid MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
