using AutoMapper;
using NetFilm.Application.DTOs.CountryDTOs;
using NetFilm.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Infrastructure.Mappers
{
	public class ProfileMapper : Profile
	{
		public ProfileMapper()
		{
			// Mapper for country
			CreateMap<Country, CountryDto>().ReverseMap();
			CreateMap<AddCountryRequestDto, Country>();
		}
	}
}
