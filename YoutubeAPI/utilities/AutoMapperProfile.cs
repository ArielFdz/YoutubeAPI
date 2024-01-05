using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using YoutubeAPI.dto;
using YoutubeExplode.Videos;

namespace YoutubeAPI.utilities
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Video, YoutubeDTO>()
                .ForMember(dest => dest.Autor, opt => opt.MapFrom(src => src.Author));
        }
    }
}