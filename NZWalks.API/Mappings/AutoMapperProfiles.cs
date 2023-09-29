using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
           CreateMap<Region, RegionDto>().ReverseMap();     //First mapping defined - GetAll method and GetById

            CreateMap<AddRegionRequestDto, Region>().ReverseMap();  //Create mapping defined - source = AddRegionRequestDto
                                                                    // Destination = Region ...Domain model
            CreateMap<UpdateRegionRequestDto, Region>().ReverseMap();   //Update method

            CreateMap<AddWalkRequestDto, Walk>().ReverseMap();  // Source is AddWalkRequestDto, Destination is Walk Domain

            CreateMap<Walk, WalkDto>().ReverseMap();

            CreateMap<Difficulty,DifficultyDto>().ReverseMap();

            CreateMap<UpdateWalkRequestDto, Walk>().ReverseMap();
        }


  


    }
}
