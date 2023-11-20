using KiteschoolService.Models;
using Microsoft.EntityFrameworkCore;

namespace KiteschoolService.Data
{
    public class KiteschoolRepo : IKiteschoolRepo
    {
        private readonly AppDbContext _context;

        public KiteschoolRepo(AppDbContext context)
        {
            _context = context;
        }

        public void CreateKiteschool(Kiteschool kiteschool)
        {
            if (kiteschool == null)
            {
                throw new ArgumentNullException(nameof(kiteschool));
            }

            _context.Kiteschools.Add(kiteschool);
        }

        // To prevent adding duplicate Kiteschools
        public bool ExternalKiteschoolExists(int externalKiteschoolId)
        {
            return _context.Kiteschools.Any(p => p.ExternalID == externalKiteschoolId);
        }

        public IEnumerable<Kiteschool> GetAllKiteschools()
        {
            return _context.Kiteschools.ToList();
        }

        public bool KiteschoolExists(int kiteschoolId)
        {
            return _context.Kiteschools.Any(p => p.Id == kiteschoolId);
        }

        public bool SaveChanges()
        {
            // return true if 1 or more entities were impacted
            return (_context.SaveChanges() >= 0);
        }

        // public void CreateCommand(int platformId, Command command)
        // {
        //     if (command == null)
        //     {
        //         throw new ArgumentNullException(nameof(command));
        //     }

        //     command.PlatformId = platformId;
        //     _context.Commands.Add(command);
        // }

        // public Command GetCommand(int platformId, int commandId)
        // {
        //     return _context.Commands
        //         .Where(c => c.PlatformId == platformId && c.Id == commandId)
        //         .FirstOrDefault();
        // }

        // public IEnumerable<Command> GetCommandsForPlatform(int platformId)
        // {
        //     return _context.Commands
        //         .Where(c => c.PlatformId == platformId)
        //         .OrderBy(c => c.Platform.Name);
        // }
    }
}