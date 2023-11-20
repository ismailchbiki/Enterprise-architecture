using System.ComponentModel.DataAnnotations;

namespace KiteschoolService.Models
{
    public class Kiteschool
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Location { get; set; }
        [Required]
        public string Email { get; set; }

        // This will be the id of the kiteschool
        // We need to map the ExternalID to the 'KiteschoolPublishedDto' Id in AutoMapper
        [Required]
        public int ExternalID { get; set; }

        // Navigation property
        // public ICollection<Command> Commands { get; set; } = new List<Command>();
    }
}