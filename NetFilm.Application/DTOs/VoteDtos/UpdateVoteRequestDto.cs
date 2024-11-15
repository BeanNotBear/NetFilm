using NetFilm.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Application.DTOs.VoteDtos
{
    public class UpdateVoteRequestDTO
    {
        [Required]
        public Guid UserId { get; set; }
        public Guid MovieId { get; set; }
        public int Star { get; set; }
    }
}
