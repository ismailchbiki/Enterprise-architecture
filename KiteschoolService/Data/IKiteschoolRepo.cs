using KiteschoolService.Models;

namespace KiteschoolService.Data
{
    public interface IKiteschoolRepo
    {
        bool SaveChanges();

        // Kiteschools
        IEnumerable<Kiteschool> GetAllKiteschools();
        void CreateKiteschool(Kiteschool kiteschool);
        bool KiteschoolExists(int kiteschoolId);
        bool ExternalKiteschoolExists(int externalKiteschoolId);

        // Commands
        // IEnumerable<Command> GetCommandsForPlatform(int platformId);
        // Command GetCommand(int platformId, int commandId);
        // void CreateCommand(int platformId, Command command);
    }
}