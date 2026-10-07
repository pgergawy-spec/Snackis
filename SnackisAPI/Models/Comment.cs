namespace SnackisAPI.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = "";
        public DateTime Date { get; set; }

        public string UserId { get; set; } = "";
        public User? User { get; set; }

        public Guid PostId { get; set; }
        public Post? Post { get; set; }
    }
}