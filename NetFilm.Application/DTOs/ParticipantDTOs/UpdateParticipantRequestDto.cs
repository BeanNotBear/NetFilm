using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Application.DTOs.ParticipantDTOs
{
    public class UpdateParticipantRequestDto
    {
        public string Name { get; set; }
        public int RoleInMovie { get; set; }
    }
}
