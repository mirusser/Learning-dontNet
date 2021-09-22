using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Google.Protobuf.Collections;
using Grpc.Core;
using PlatformService.Data;

namespace PlatformService.SyncDataServices.Grpc
{
    public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
    {
        private readonly IPlatformRepo _repo;
        private readonly IMapper _mapper;

        public GrpcPlatformService(IPlatformRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public override Task<PlatformReply> GetAllPlaforms(GettAllRequest request, ServerCallContext context)
        {
            var platforms = _repo.GetAtllPlatforms();
            var grpcPlatformReplies = _mapper.Map<RepeatedField<GrpcPlatformReply>>(platforms);

            var platformReply = new PlatformReply();
            platformReply.Platforms.AddRange(grpcPlatformReplies);

            return Task.FromResult(platformReply);
        }
    }
}