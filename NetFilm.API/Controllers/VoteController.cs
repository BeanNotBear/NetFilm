﻿using Microsoft.AspNetCore.Mvc;
using NetFilm.Application.Attributes;
using NetFilm.Application.DTOs.CountryDTOs;
using NetFilm.Application.Interfaces;
using static System.Net.Mime.MediaTypeNames;
using NetFilm.Application.DTOs.VoteDtos;

namespace NetFilm.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly IVoteService voteService;

        public VoteController(IVoteService voteService)
        {
            this.voteService = voteService;
        }

        /// <summary>
        /// Get all countries
        /// </summary>
        /// <returns>list of countries</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var vote = await voteService.GetAll();
            return Ok(vote);
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
            var vote = await voteService.GetById(id);
            return Ok(vote);
        }

        /// <summary>
        /// Add a new country
        /// </summary>
        /// <param name="addCountryRequestDto">add country request</param>
        /// <returns>country</returns>
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Add([FromBody] AddVoteRequestDTO addVoteRequestDto)
        {
            var vote = await voteService.Add(addVoteRequestDto);
            return CreatedAtAction(nameof(GetById), new { id = vote.UserId }, vote);
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
            await voteService.HardDelete(id);
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
            var vote = await voteService.SoftDelete(id);
            return Ok(vote);
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
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateVoteRequestDTO updateVoteRequestDTO)
        {
            var vote = await voteService.Update(id, updateVoteRequestDTO);
            return Ok(vote);
        }
    }
}