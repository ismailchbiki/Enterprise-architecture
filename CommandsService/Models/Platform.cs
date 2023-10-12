using System.ComponentModel.DataAnnotations;

namespace CommandsService.Models
{
    public class Platform
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        // This will be the id of the platform
        // We need to map the ExternalID to the 'PlatformPublishedDto' Id in AutoMapper
        [Required]
        public int ExternalID { get; set; }

        // Navigation property
        public ICollection<Command> Commands { get; set; } = new List<Command>();
    }
}