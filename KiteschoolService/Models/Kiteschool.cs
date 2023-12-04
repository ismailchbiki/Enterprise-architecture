using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace KiteschoolService.Models
{
    public class Kiteschool
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Location { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public int CreatedByUserId { get; set; }
    }
}