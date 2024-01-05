using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using YoutubeAPI.dto;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace YoutubeAPI.Controllers
{
    public class downloaderService
    {
        private readonly IMapper mapper;

        public downloaderService(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public async Task<YoutubeDTO> m_getMetadataVideo(string _urlVideo)
        {

            try
            {
                var youtube = new YoutubeClient();
                var metadata = await youtube.Videos.GetAsync(_urlVideo);
                var metadataDTO = mapper.Map<YoutubeDTO>(metadata);

                return metadataDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar procesar la url proporcionada: " + ex.Message.ToString());
            }
        }

        public async Task<VideoBuffer> m_getVideoBuffer(string _urlVideo)
        {
            var buffer = new VideoBuffer();

            try
            {
                var youtube = new YoutubeClient();
                var streamManifest = await youtube.Videos.Streams.GetManifestAsync(_urlVideo);
                var streamInfo = streamManifest.GetMuxedStreams().GetWithHighestVideoQuality();
                var stream = await youtube.Videos.Streams.GetAsync(streamInfo);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    buffer.buffer = memoryStream.ToArray();

                    return buffer;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar procesar la url proporcionada: " + ex.Message.ToString());
            }

        }

        public async Task<dynamic> m_downloadVideo(string _urlVideo)
        {

            try
            {
                var youtube = new YoutubeClient();
                var streamManifest = await youtube.Videos.Streams.GetManifestAsync(_urlVideo);
                var streamInfo = streamManifest.GetMuxedStreams().GetWithHighestVideoQuality();
                var stream = await youtube.Videos.Streams.GetAsync(streamInfo);

                return stream;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar procesar la url proporcionada: " + ex.Message.ToString());
            }

        }
    }
}