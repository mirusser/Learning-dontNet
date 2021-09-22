using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CommandsService.Models;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using PlatformService;

namespace CommandsService.SyncDataServices.Grpc
{
    public class PlatformDataClient : IPlatformDataClient
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public PlatformDataClient(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            Console.WriteLine($"--> Calling Grpc Service: {_configuration["GrpcPlatform"]}");

            var platforms = new List<Platform>();
            var channel = GrpcChannel.ForAddress(_configuration["GrpcPlatform"]);
            var client = new GrpcPlatform.GrpcPlatformClient(channel);
            var request = new GettAllRequest();

            try
            {
                var response = client.GetAllPlaforms(request);
                platforms = _mapper.Map<List<Platform>>(response.Platforms);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not call Grpc Server: {ex.Message}");
            }

            return platforms;
        }
    }
}