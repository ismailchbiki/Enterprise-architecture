using System.ComponentModel.DataAnnotations;

namespace UserService.Dtos
{
    public class KiteschoolCreateDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string Email { get; set; }
    }
}