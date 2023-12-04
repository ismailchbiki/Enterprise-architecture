using KiteschoolService.Models;

namespace KiteschoolService.Data
{
    public interface IKiteschoolRepo
    {
        void CreateKiteschool(Kiteschool kiteschool);
        void CreateManyKiteschools(IEnumerable<Kiteschool> kiteschools);
        Kiteschool GetKiteschoolById(string id);
        IEnumerable<Kiteschool> GetAllKiteschools();
        IEnumerable<Kiteschool> GetKiteschoolsByUserId(int userId);
    }
}