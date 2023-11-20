using AutoMapper;
using Grpc.Core;
using UserService.Data;

namespace UserService.SyncDataServices.Grpc
{
    // Setup the gRPC service (server).
    // It will be used by any client that calls the gRPC service.
    // In this case KiteschoolService will be the client and will need to get data synchronously from UserService about the new kiteschools.
    public class GrpcUserService : GrpcKiteschool.GrpcKiteschoolBase
    {
        private readonly IKiteschoolRepo _repository;
        private readonly IMapper _mapper;

        public GrpcUserService(IKiteschoolRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // This method is called by the client (KiteschoolService) to get all kiteschools.
        public override Task<KiteschoolResponse> GetAllKiteschools(GetAllRequest request, ServerCallContext context)
        {
            var response = new KiteschoolResponse();

            var kiteschools = _repository.GetAllKiteschools();

            foreach (var kiteschool in kiteschools)
            {
                response.Kiteschool.Add(_mapper.Map<GrpcKiteschoolModel>(kiteschool));
            }

            return Task.FromResult(response);
        }
    }
}