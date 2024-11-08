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
        public int Star { get; set; }
    }
}
