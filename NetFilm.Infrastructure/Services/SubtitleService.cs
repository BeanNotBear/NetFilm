using AutoMapper;
using NetFilm.Application.DTOs.SubtitleDTOs;
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
	}
}
