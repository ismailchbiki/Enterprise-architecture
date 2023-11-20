namespace KiteschoolService.EventProcessing
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);
    }
}