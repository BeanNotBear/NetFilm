﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Application.DTOs.AuthDTOs
{
    public class VerifyEmailDto
    {
        public string email { get; set; }
        public string code { get; set; }
    }
}