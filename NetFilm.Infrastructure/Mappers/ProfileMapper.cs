using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NetFilm.Application.DTOs.AuthDTOs;
using NetFilm.Application.DTOs.CountryDTOs;
using NetFilm.Application.DTOs.MovieDTOs;
using NetFilm.Application.DTOs.SubtitleDTOs;
using NetFilm.Domain.Common;
using NetFilm.Application.DTOs.RoleDTOs;
using NetFilm.Application.DTOs.UserDTOs;
using NetFilm.Application.DTOs.ParticipantDTOs;
using NetFilm.Application.DTOs.VoteDtos;
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

			//Mapper for Role
			CreateMap<IdentityRole<Guid>, RoleDto>()
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
			.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
			.ReverseMap();
			// Mapper for Category
			CreateMap<Category, CategoryDto>().ReverseMap();
			CreateMap<Category, ChangeCategoryDto>().ReverseMap();

			// Mapper for Commnet
			CreateMap<Comment, CommentDto>()
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
			CreateMap<PagedResult<Movie>, PagedResult<MovieDto>>();
			CreateMap<Movie, MovieDetailDto>()
				.ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
				.ForMember(dest => dest.Subtitles, opt => opt.MapFrom(src => src.Subtitles))
				.ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.MovieCategories.Select(x => x.Category)))
				.ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
				.ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.MovieParticipants.Select(x => x.Participant)))
				.ForMember(dest => dest.TotalPeopleVote, opt => opt.MapFrom(src => src.Votes.Count)).ReverseMap();
			CreateMap<Movie, MovieViewerDto>();
			CreateMap<PagedResult<Movie>, PagedResult<MovieViewerDto>>();


			// Mapper for subtitle
			CreateMap<Subtitle, SubtitleDto>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)).ReverseMap();

			//Mapper for User
			// AddUserRequestDto -> User
			CreateMap<AddUserRequestDto, User>()
				.ForMember(dest => dest.Id, opt => opt.Ignore())
				.ForMember(dest => dest.NormalizedUserName, opt => opt.Ignore())
				.ForMember(dest => dest.NormalizedEmail, opt => opt.Ignore())
				.ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => false))
				.ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
				.ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
				.ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore())
				.ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.MapFrom(src => false))
				.ForMember(dest => dest.TwoFactorEnabled, opt => opt.MapFrom(src => false))
				.ForMember(dest => dest.LockoutEnd, opt => opt.Ignore())
				.ForMember(dest => dest.LockoutEnabled, opt => opt.MapFrom(src => true))
				.ForMember(dest => dest.AccessFailedCount, opt => opt.MapFrom(src => 0))
				// Handle password casing difference
				.ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.PassWord))
				.ReverseMap();

			// User -> UserDto
			CreateMap<User, UserDto>().ReverseMap();

			// Map from UpdateUserRequestDto to User
			CreateMap<UpdateUserRequestDto, User>()
				.ForMember(dest => dest.Id, opt => opt.Ignore())
				.ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
				.ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore())
				.ForMember(dest => dest.LockoutEnd, opt => opt.Ignore())
				.ForMember(dest => dest.LockoutEnabled, opt => opt.Ignore())
				.ForMember(dest => dest.AccessFailedCount, opt => opt.Ignore())
				.ForMember(dest => dest.EmailConfirmed, opt => opt.Ignore())
				.ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.Ignore())
				.ForMember(dest => dest.TwoFactorEnabled, opt => opt.Ignore())
				.ForMember(dest => dest.NormalizedEmail, opt => opt.Ignore())
				.ForMember(dest => dest.NormalizedUserName, opt => opt.Ignore())
				.ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
				.ReverseMap();


			// Map RegisterRequestDto with User
			CreateMap<RegisterRequestDto, User>()
			.ForMember(dest => dest.Id, opt => opt.Ignore())
			.ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
			.ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
			.ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
			.ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
			.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
			.ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
			// Ignore additional IdentityUser properties that should be set separately
			.ForMember(dest => dest.NormalizedUserName, opt => opt.Ignore())
			.ForMember(dest => dest.NormalizedEmail, opt => opt.Ignore())
			.ForMember(dest => dest.EmailConfirmed, opt => opt.Ignore())
			.ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
			.ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
			.ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore())
			.ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.Ignore())
			.ForMember(dest => dest.TwoFactorEnabled, opt => opt.Ignore())
			.ForMember(dest => dest.LockoutEnd, opt => opt.Ignore())
			.ForMember(dest => dest.LockoutEnabled, opt => opt.Ignore())
			.ForMember(dest => dest.AccessFailedCount, opt => opt.Ignore())
			.ReverseMap();

			// Mapper for Advertise
			CreateMap<Advertise, AdvertiseDto>()
				.ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.User.LastName))
				.ReverseMap();
			CreateMap<Advertise, AddAdvertiseDto>().ReverseMap();
			CreateMap<Advertise, UpdateAdvertiseDto>().ReverseMap();

			CreateMap<Participant, ParticipantDto>().ReverseMap();
			CreateMap<AddParticipantRequestDto, Participant>();

			CreateMap<Vote, VoteDto>().ReverseMap();
			CreateMap<AddVoteRequestDTO, Vote>();
		}
	}
}
