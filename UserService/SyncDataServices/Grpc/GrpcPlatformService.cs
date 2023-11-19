using AutoMapper;
using Grpc.Core;
using UserService.Data;

namespace UserService.SyncDataServices.Grpc
{
    // Setup the gRPC service (server).
    // It will be used by any client that calls the gRPC service.
    // In this case CommandService will be the client and will need to get data synchronously from UserService about the new platforms.
    public class GrpcUserService : GrpcPlatform.GrpcPlatformBase
    {
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;

        public GrpcUserService(IPlatformRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // This method is called by the client (CommandService) to get all platforms.
        public override Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
        {
            var response = new PlatformResponse();

            var platforms = _repository.GetAllPlatforms();

            foreach (var plat in platforms)
            {
                response.Platform.Add(_mapper.Map<GrpcPlatformModel>(plat));
            }

            return Task.FromResult(response);
        }
    }
}