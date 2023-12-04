namespace UserService.Dtos
{
    public class KiteschoolPublishedDto
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public int CreatedByUserId { get; set; }

        public string Event { get; set; }
    }
}