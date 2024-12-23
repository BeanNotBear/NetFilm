﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFilm.Application.Attributes;

namespace NetFilm.Application.DTOs.UserDTOs
{
    public class AddUserRequestDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [DateOfBirthModel]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        [CustomEmail]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Password]
        public string PassWord { get; set; }
        [Required]
        [Phone]
        [PhoneNumber]
        public string PhoneNumber { get; set; }
        public string[] Roles { get; set; }
    }

}
