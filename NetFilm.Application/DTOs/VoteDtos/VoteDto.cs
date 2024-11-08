using NetFilm.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Application.DTOs.VoteDtos
{
    public class VoteDto
    {
        public Guid MovieId { get; set; }
        public Guid UserId { get; set; }

        [Range(0, 6)]
        public int Star { get; set; }
    }
}
