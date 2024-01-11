using KiteschoolService.EventProcessing;

namespace KiteschoolServiceTests.Mocks
{
    public class MockEventProcessor : IEventProcessor
    {
        public void ProcessEvent(string message)
        {
            // Mock the event processing logic
        }
    }
}