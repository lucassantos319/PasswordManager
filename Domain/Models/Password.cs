namespace Domain.Models
{
    public class PasswordModel
    {
        public DateTime CreatedAt { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int TypeId { get; set; }
    }
}