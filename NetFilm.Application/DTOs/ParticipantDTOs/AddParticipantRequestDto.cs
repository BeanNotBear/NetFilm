using NetFilm.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Application.DTOs.ParticipantDTOs
{
    public class AddParticipantRequestDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(0, 2)]
        public RoleInMovie RoleInMovie { get; set; }
    }
}
