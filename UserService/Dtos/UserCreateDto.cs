using System.ComponentModel.DataAnnotations;

namespace UserService.Dtos
{
    public class UserCreateDto
    {
        [Required]
        [MaxLength(250)]
        public string Firstname { get; set; }
        [Required]
        [MaxLength(250)]
        public string Lastname { get; set; }
        [Required]
        [MaxLength(250)]
        public string Email { get; set; }
        [Required]
        [MaxLength(250)]
        public string Password { get; set; }
        [Required]
        [MaxLength(250)]
        public string Role { get; set; }
    }
}