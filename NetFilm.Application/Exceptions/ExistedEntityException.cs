﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Application.Exceptions
{
	public class ExistedEntityException : Exception
	{
		public ExistedEntityException(string? message) : base(message)
		{
		}
	}
}
