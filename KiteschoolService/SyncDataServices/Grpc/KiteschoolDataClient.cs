using AutoMapper;
using KiteschoolService.Models;
using Grpc.Net.Client;
using UserService;

namespace KiteschoolService.SyncDataServices.Grpc
{
    public class KiteschoolDataClient : IKiteschoolDataClient
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public KiteschoolDataClient(IConfiguration config, IMapper mapper)
        {
            _config = config;
            _mapper = mapper;
        }

        public IEnumerable<Kiteschool> ReturnAllKiteschools()
        {
            Console.WriteLine($"--> Calling Grpc Service {_config["GrpcKiteschool"]}");

            // This will try to fetch kiteschools from the gRPC server (UserService).
            var channel = GrpcChannel.ForAddress(_config["GrpcKiteschool"]);
            var client = new GrpcKiteschool.GrpcKiteschoolClient(channel);
            var request = new GetAllRequest();
            try
            {
                var reply = client.GetAllKiteschools(request);
                return _mapper.Map<IEnumerable<Kiteschool>>(reply.Kiteschool);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not call Grpc Server {ex.Message}");
                return null;
            }
        }
    }
}