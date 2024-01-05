using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using YoutubeAPI.dto;
using YoutubeExplode;

namespace YoutubeAPI.Controllers
{
    public class downloaderService
    {
        private readonly IMapper mapper;

        public downloaderService(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public async Task<YoutubeDTO> m_getMetadataVideo(string _urlVideo){

            try{
                var youtube = new YoutubeClient();
                var metadata = await youtube.Videos.GetAsync(_urlVideo);
                var metadataDTO = mapper.Map<YoutubeDTO>(metadata);

                return metadataDTO;
            }
            catch(Exception ex)
            {
                throw new Exception("Error al intentar procesar la url proporcionada: " + ex.Message.ToString());
            }    
        }
    }
}