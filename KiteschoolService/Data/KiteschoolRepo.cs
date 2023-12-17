using KiteschoolService.Exceptions;
using KiteschoolService.Models;
using MongoDB.Driver;

namespace KiteschoolService.Data
{
    public class KiteschoolRepo : IKiteschoolRepo
    {
        private readonly IMongoDatabase _mongoDatabase;

        public KiteschoolRepo(IMongoDatabase mongoDatabase)
        {
            _mongoDatabase = mongoDatabase;
        }

        public void CreateKiteschool(Kiteschool kiteschool)
        {
            try
            {
                if (kiteschool == null)
                {
                    throw new ArgumentNullException(nameof(kiteschool));
                }

                Console.WriteLine("--> Creating new kiteschool");
                _mongoDatabase.GetCollection<Kiteschool>("Kiteschools").InsertOne(kiteschool);
            }
            catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                Console.WriteLine($"Error in CreateKiteschool. Duplicate key error: {ex.Message}", ex);
                throw new DuplicateKeyException("Duplicate key error. The provided Kiteschool already exists.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateKiteschool. An unexpected error occurred: {ex.Message}", ex);
                throw new KiteschoolCreationException("An unexpected error occurred while creating the Kiteschool.", ex);
            }
        }

        public void CreateManyKiteschools(IEnumerable<Kiteschool> kiteschools)
        {
            _mongoDatabase.GetCollection<Kiteschool>("Kiteschools").InsertMany(kiteschools);
        }

        public Kiteschool GetKiteschoolById(string id)
        {
            try
            {
                return _mongoDatabase.GetCollection<Kiteschool>("Kiteschools").Find(k => k.Id == id).FirstOrDefault();
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Error in GetKiteschoolById. Invalid ObjectId format for value: '{id}'.", ex);
                throw new FormatException("Invalid ObjectId format. The provided value is not a valid 24-digit hex string.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetKiteschoolById. An unexpected error occurred: {ex.Message}", ex);
                throw;
            }
        }

        public IEnumerable<Kiteschool> GetAllKiteschools()
        {
            return _mongoDatabase.GetCollection<Kiteschool>("Kiteschools").AsQueryable();
        }

        public IEnumerable<Kiteschool> GetKiteschoolsByUserId(int userId)
        {
            return _mongoDatabase.GetCollection<Kiteschool>("Kiteschools")
                .AsQueryable()
                .Where(kiteschool => kiteschool.CreatedByUserId == userId)
                .ToList();
        }
    }
}