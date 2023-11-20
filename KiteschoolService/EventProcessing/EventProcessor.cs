using System.Text.Json;
using AutoMapper;
using KiteschoolService.Data;
using KiteschoolService.Dtos;
using KiteschoolService.Models;

namespace KiteschoolService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory serviceScopeFactory, IMapper mapper)
        {
            _scopeFactory = serviceScopeFactory;
            _mapper = mapper;
        }

        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.KiteschoolPublished:
                    AddKiteschool(message);
                    break;
                default:
                    break;
            }
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine($"--> Determining Event");

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

            switch (eventType.Event)
            {
                case "Kiteschool_Published":
                    Console.WriteLine("--> Kiteschool Published Event Detected");
                    return EventType.KiteschoolPublished;
                default:
                    Console.WriteLine("--> Could not determine the event type");
                    return EventType.Undetermined;
            }
        }

        private void AddKiteschool(string message)
        {
            // Create a new scope using the _scopeFactory, which is likely used for managing dependencies.
            using (var scope = _scopeFactory.CreateScope())
            {
                // Get an instance of IKiteschoolRepo from the service provider within the created scope.
                var repo = scope.ServiceProvider.GetRequiredService<IKiteschoolRepo>();

                // Deserialize the 'message' parameter into a KiteschoolPublishedDto object using JSON deserialization.
                var kiteschoolPublishedDto = JsonSerializer.Deserialize<KiteschoolPublishedDto>(message);

                try
                {
                    var plat = _mapper.Map<Kiteschool>(kiteschoolPublishedDto);

                    if (!repo.ExternalKiteschoolExists(plat.ExternalID))
                    {
                        repo.CreateKiteschool(plat);
                        repo.SaveChanges();

                        Console.WriteLine("--> Kiteschool added!");
                    }
                    else
                    {
                        Console.WriteLine("--> Kiteschool already exists...");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Kiteschool to DB {ex.Message}");
                }
            }
        }

    }

    enum EventType
    {
        KiteschoolPublished,
        Undetermined
    }
}
