using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetFilm.Application.Attributes;
using NetFilm.Application.DTOs.CountryDTOs;
using NetFilm.Application.DTOs.ParticipantDTOs;
using NetFilm.Application.Interfaces;
using NetFilm.Infrastructure.Services;

namespace NetFilm.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantsController : ControllerBase
    {
        private readonly IParticipantService _participantService;

        public ParticipantsController(IParticipantService participantService)
        {
            this._participantService = participantService;
        }

        /// <summary>
        /// Get all countries
        /// </summary>
        /// <returns>list of countries</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var participant = await _participantService.GetAll();
            return Ok(participant);
        }


        /// <summary>
        /// Get country by id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>country</returns>
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var participant = await _participantService.GetById(id);
            return Ok(participant);
        }

        /// <summary>
        /// Add a new country
        /// </summary>
        /// <param name="addCountryRequestDto">add country request</param>
        /// <returns>country</returns>
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Add([FromBody] AddParticipantRequestDto addParticipantRequestDto)
        {
            var participant = await _participantService.Add(addParticipantRequestDto);
            return CreatedAtAction(nameof(GetById), new { id = participant.Id }, participant);
        }

        /// <summary>
        /// Hard delete a country
        /// </summary>
        /// <param name="id">id of country</param>
        /// <returns>no content</returns>
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> HardDelete([FromRoute] Guid id)
        {
            await _participantService.HardDelete(id);
            return NoContent();
        }

        /// <summary>
        /// Soft delete a country
        /// </summary>
        /// <param name="id">id of country</param>
        /// <returns>country</returns>
        [HttpPatch]
        [Route("{id:guid}")]
        public async Task<IActionResult> SoftDelete([FromRoute] Guid id)
        {
            var participant = await _participantService.SoftDelete(id);
            return Ok(participant);
        }

        /// <summary>
        /// Update a country
        /// </summary>
        /// <param name="id">id of country</param>
        /// <param name="updateCountryRequestDto">new data need update</param>
        /// <returns>country</returns>
        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateParticipantRequestDto updateParticipantRequestDto)
        {
            var participant = await _participantService.Update(id, updateParticipantRequestDto);
            return Ok(participant);
        }
    }
}