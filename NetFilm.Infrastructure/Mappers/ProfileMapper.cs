using AutoMapper;
using NetFilm.Application.DTOs.CountryDTOs;
using NetFilm.Application.DTOs.MovieDTOs;
using NetFilm.Application.DTOs.SubtitleDTOs;
using NetFilm.Domain.Entities;

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

			// Mapper for Movie
			CreateMap<Movie, MovieDto>().ReverseMap();
			CreateMap<AddMovieRequestDto, Movie>();

			// Mapper for subtitle
			CreateMap<Subtitle, SubtitleDto>().ReverseMap();
		}
	}
}
