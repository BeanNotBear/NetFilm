﻿using NetFilm.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Domain.Interfaces
{
	public interface IMovieParticipantRepository : IBaseRepository<MovieParticipant, Guid>
	{

	}
}
