using UserService.Models;

namespace UserService.Data
{
    public class KiteschoolRepo : IKiteschoolRepo
    {
        private readonly AppDbContext _appDbContext;

        public KiteschoolRepo(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void CreateKiteschool(Kiteschool kiteschool)
        {
            if (kiteschool == null)
            {
                throw new ArgumentNullException(nameof(kiteschool));
            }

            _appDbContext.Kiteschools.Add(kiteschool);
        }

        public IEnumerable<Kiteschool> GetAllKiteschools()
        {
            return _appDbContext.Kiteschools.ToList();
        }

        public Kiteschool GetKiteschoolById(int id)
        {
            return _appDbContext.Kiteschools.FirstOrDefault(p => p.Id == id);
        }

        public bool SaveChanges()
        {
            return _appDbContext.SaveChanges() >= 0;
        }
    }
}