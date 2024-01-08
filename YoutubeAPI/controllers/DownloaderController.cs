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

        [HttpGet("metadata")]
        public async Task<ActionResult> m_getMetadataVideo(string _urlVideo)
        {

            try
            {
                var metadata = await service.m_getMetadataVideo(_urlVideo);
                return Ok(metadata);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            }

        }

        [HttpGet("buffer")]
        public async Task<ActionResult> m_getVideoBuffer(string _urlVideo)
        {
            try
            {
                var buffer = await service.m_getVideoBuffer(_urlVideo);
                return Ok(buffer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            }
        }

        [HttpGet("video")]
        public async Task<ActionResult> m_downloadVideo(string _urlVideo)
        {
            try
            {
                var stream = await service.m_downloadVideo(_urlVideo);
                var metadata = await service.m_getMetadataVideo(_urlVideo);
                
                Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{metadata.Title}.mp4\"");
                
                return Ok(stream);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            }
        }

        [HttpGet("audio")]
        public async Task<ActionResult> m_downloadAudio(string _urlVideo)
        {
            try
            {
                var stream = await service.m_downloadAudio(_urlVideo);
                var metadata = await service.m_getMetadataVideo(_urlVideo);
                
                Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{metadata.Title}.mp3\"");
                
                return Ok(stream);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            }
        }
    }
}