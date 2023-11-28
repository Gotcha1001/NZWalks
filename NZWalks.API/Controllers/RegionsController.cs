using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Text.Json;

namespace NZWalks.API.Controllers
{

    // https://localhost:7047/api/regions

    [Route("api/[controller]")]
    [ApiController]
 
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository,
            IMapper mapper, ILogger<RegionsController> logger)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        //GET ALL REGIONS
        //GET : https://localhost:7047/api/regions
        [HttpGet]
       // [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery)
        {
            

                //Get Data From Database - Domain Models
                var regionsDomain = await regionRepository.GetAllAsync(filterOn, filterQuery);

                // Return Dto's

                logger.LogInformation($"Finished GetAllRegions request with data: {JsonSerializer.Serialize(regionsDomain)}");

                return Ok(mapper.Map<List<RegionDto>>(regionsDomain));
        
        }

        //GET (Get Region by ID)
        //GET : https://localhost:7047/api/regions/{id} 
        [HttpGet]
        [Route("{id:Guid}")]
       // [Authorize(Roles = "Reader")]

        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            // var region = dbContext.Regions.Find(id);     other way of doing it
            //Get Region Domain Model from database
            var regionDomain = await regionRepository.GetByIdAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }
     
            // Return Dto's back to client
            return Ok(mapper.Map<RegionDto>(regionDomain));
        }

        //POST To create a new Region
        //POST : https://localhost:7047/api/regions
        [HttpPost]
        [ValidateModel]
       // [Authorize(Roles = "Writer")]

        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            
                //Map or Convert DTO to Domain Model
                var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

                //Use Domain Model to create Region
                regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

                //Map Domain Model back to DTO
                var regionDto = mapper.Map<RegionDto>(regionDomainModel);

                return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
            
          
        }

        //Update region
        //PUT :  https://localhost:7047/api/regions/{id} 
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
       // [Authorize(Roles = "Writer")]

        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            
                //Map Dto to Domain Model
                var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

                // Check if region exists
                regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

                //check if its null or not
                if (regionDomainModel == null)
                {
                    return NotFound();
                }

                return Ok(mapper.Map<RegionDto>(regionDomainModel));
            
          

        }

        // Delete Region
        // DELETE :  https://localhost:7047/api/regions/{id} 
        [HttpDelete]
        [Route("{id:Guid}")]
      // [Authorize(Roles = "Writer,Reader")]

        public async Task<IActionResult> DeleteById([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);


            if(regionDomainModel == null)
            {
                return NotFound();
            }

            // return deleted Region back
            // Map Domain Model to DTO first
            return Ok(mapper.Map<RegionDto>(regionDomainModel));

        }
    }
}
