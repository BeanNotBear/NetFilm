using AutoMapper;
using NetFilm.Application.DTOs.CountryDTOs;
using NetFilm.Application.DTOs.MovieDTOs;
using NetFilm.Application.DTOs.SubtitleDTOs;
using NetFilm.Domain.Entities;
using NetFilm.Application.DTOs.MovieCategoryDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFilm.Application.DTOs.CategoryDtos;
using NetFilm.Application.DTOs.CommentDTOs;
using NetFilm.Application.DTOs.AdvertiseDTOs;

namespace NetFilm.Infrastructure.Mappers
{
	public class ProfileMapper : Profile
	{
		public ProfileMapper()
		{
			// Mapper for country
			CreateMap<Country, CountryDto>().ReverseMap();
			CreateMap<AddCountryRequestDto, Country>();

			// Mapper for Category
			CreateMap<Category,CategoryDto>().ReverseMap();
			CreateMap<Category,ChangeCategoryDto>().ReverseMap();

			// Mapper for Commnet
			CreateMap<Comment,CommentDto>()
				.ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.LastName))
                .ForMember(dest => dest.Movie, opt => opt.MapFrom(src => src.Movie.Name))
                .ReverseMap();
			CreateMap<Comment, AddCommentDto>().ReverseMap();
			CreateMap<Comment, UpdateCommentDto>().ReverseMap();
			CreateMap<Comment, ReplyDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.LastName))
                .ForMember(dest => dest.Movie, opt => opt.MapFrom(src => src.Movie.Name))
				.ReverseMap();
			//Mapper for User

			// Mapper for Movie
			CreateMap<Movie, MovieDto>().ReverseMap();
			CreateMap<AddMovieRequestDto, Movie>();

			// Mapper for subtitle
			CreateMap<Subtitle, SubtitleDto>().ReverseMap();

			// Mapper for Advertise
			CreateMap<Advertise,AdvertiseDto>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.User.LastName))
				.ReverseMap();
			CreateMap<Advertise,AddAdvertiseDto>().ReverseMap();
			CreateMap<Advertise,UpdateAdvertiseDto>().ReverseMap();
		}
	}
}
