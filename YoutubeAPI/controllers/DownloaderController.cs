using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace YoutubeAPI.Controllers
{
    [ApiController]
    [Route("api/downloader")]
    public class downloaderController : ControllerBase
    {
        private readonly downloaderService service;

        public downloaderController(downloaderService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult> m_getMetadataVideo(string _urlVideo){
            try
            {
                var metadata = await service.m_getMetadataVideo(_urlVideo);
                return Ok(metadata);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            }
            
        }
    }
}