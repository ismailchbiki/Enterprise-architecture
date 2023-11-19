using System.ComponentModel.DataAnnotations;

namespace UserService.Models
{
    public class Kiteschool
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string Email { get; set; }
    }
}