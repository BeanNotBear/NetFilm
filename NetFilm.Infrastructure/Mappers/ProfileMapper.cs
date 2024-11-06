using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NetFilm.Application.DTOs.CountryDTOs;
using NetFilm.Application.DTOs.RoleDTOs;
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

			//Mapper for User
			CreateMap<IdentityRole<Guid>, RoleDto>()
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
			.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
			.ReverseMap();

        }
	}
}
