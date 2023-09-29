﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Runtime.CompilerServices;

namespace NZWalks.API.Controllers
{
    // /api/walks            If you reach this endpoint you will reach this Controller
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        // CREATE Walk
        // POST: /api/walks

        [HttpPost]
        [ValidateModel]

        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            
                // Map Dto to Domain Model  : AddWalkRequestDto to Walk.cs Domain Model

                var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

                await walkRepository.CreateAsync(walkDomainModel);

                // Map Domain model to DTO 
                return Ok(mapper.Map<WalkDto>(walkDomainModel));
            
        }

        // GET walks
        // GET: /api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&IsAscending=true&pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var walksDomainModel = await walkRepository.GetAllAsync(filterOn, filterQuery, sortBy,
                isAscending ?? true, pageNumber, pageSize);
            // Map Domain Model to DTO
            return Ok(mapper.Map<List<WalkDto>>(walksDomainModel));
        }

        // Get walk by Id
        // GET : /api/Walks/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.GetByIdAsync(id);

            if (walkDomainModel == null)
            {
                return NotFound();
            }

            // Map Domain Model to Dto
            return Ok(mapper.Map<WalkDto>(walkDomainModel));


        }


        // Update Walk By Id
        // PUT : /api/Walks/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {
           
                // Map DTO to Domain Model
                var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);

                await walkRepository.UpdateAsync(id, walkDomainModel);

                if (walkDomainModel == null)
                {
                    return NotFound();
                }

                return Ok(mapper.Map<WalkDto>(walkDomainModel));

        }


        // Delete A Walk By Id
        // DELETE: /api/Walks/{id}
        [HttpDelete]
        [Route("{id:Guid}")]

        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deleteWalkDomainModel = await walkRepository.DeleteAsync(id);
            if (deleteWalkDomainModel == null)
            {
                return NotFound();
            }

            //Map Domain model to Dto
            return Ok(mapper.Map<WalkDto>(deleteWalkDomainModel));
        }
    }
}