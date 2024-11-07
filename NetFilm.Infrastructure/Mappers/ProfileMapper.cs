﻿using AutoMapper;
using NetFilm.Application.DTOs.CountryDTOs;
using NetFilm.Domain.Entities;
using NetFilm.Application.DTOs.MovieCategoryDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFilm.Application.DTOs.CategoryDtos;

namespace NetFilm.Infrastructure.Mappers
{
	public class ProfileMapper : Profile
	{
		public ProfileMapper()
		{
			// Mapper for country
			CreateMap<Country, CountryDto>().ReverseMap();
			CreateMap<AddCountryRequestDto, Country>();
			CreateMap<Category,CategoryDto>().ReverseMap();
			CreateMap<Category,ChangeCategoryDto>().ReverseMap();

			//Mapper for User

		}
	}
}
