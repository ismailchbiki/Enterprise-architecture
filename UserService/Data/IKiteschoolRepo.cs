using UserService.Models;

namespace UserService.Data
{
    public interface IKiteschoolRepo
    {
        bool SaveChanges();
        IEnumerable<Kiteschool> GetAllKiteschools();
        Kiteschool GetKiteschoolById(int id);
        void CreateKiteschool(Kiteschool kiteschool);
    }
}