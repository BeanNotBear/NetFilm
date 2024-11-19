using AutoMapper;
using NetFilm.Application.DTOs.SubtitleDTOs;
using NetFilm.Application.Exceptions;
using NetFilm.Application.Interfaces;
using NetFilm.Domain.Entities;
using NetFilm.Domain.Interfaces;

namespace NetFilm.Infrastructure.Services
{
	public class SubtitleService : ISubtitleService
	{
		private readonly ISubtitleRepository subtitleRepository;
		private readonly IMapper mapper;

		public SubtitleService(ISubtitleRepository subtitleRepository, IMapper mapper)
		{
			this.subtitleRepository = subtitleRepository;
			this.mapper = mapper;
		}

		public async Task<SubtitleDto> AddSubtitle(string subtitleName, string subtitleUrl, Guid movieId)
		{
			var subtitle = new Subtitle
			{
				SubtitleName = subtitleName,
				SubtitleUrl = subtitleUrl,
				MovieId = movieId
			};
			var subtitleDDomain = await subtitleRepository.AddAsync(subtitle);
			var subtitleDto = mapper.Map<SubtitleDto>(subtitleDDomain);
			return subtitleDto;
		}

		public async Task<bool> DeleteSubtitle(Guid id)
		{
			var isDeletd = await subtitleRepository.DeleteAsync(id);
			if(!isDeletd)
			{
				throw new NotFoundException("Can not found by id");
			}
			return isDeletd;
		}

		public async Task<SubtitleDto> GetSubtitleById(Guid id)
		{
			var subtitles = await subtitleRepository.GetSubtitlebyId(id);
			if (subtitles == null)
			{
				throw new NotFoundException("Can not found");
			}
			var subtitleDto = mapper.Map<SubtitleDto>(subtitles);
			return subtitleDto;
		}
	}
}
