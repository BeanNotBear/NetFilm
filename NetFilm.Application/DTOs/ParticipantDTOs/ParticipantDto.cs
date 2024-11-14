using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetFilm.Domain.Common;

namespace NetFilm.Application.DTOs.ParticipantDTOs
{
    public class ParticipantDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public RoleInMovie RoleInMovie { get; set; }
        public bool IsDelete { get; set; }
    }
}
