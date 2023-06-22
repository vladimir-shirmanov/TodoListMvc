namespace TodoList.Models
{
    public class Category
    {
        public int Id { get; set; }

        public required string Title { get; set; }

        public required string Description { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public int UserId { get; set; }
    }
}
