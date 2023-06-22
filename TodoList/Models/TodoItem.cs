namespace TodoList.Models
{
    public class TodoItem
    {
        public int Id { get; set; }

        public required string Title { get; set; }

        public bool IsCompleted { get; set; }

        public required string Description { get; set; }

        public Category? Category { get; set; }

        public int CategoryId { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        public DateTime DueDate { get; set;}

        public DateTime? CompletedDate { get; set; }

        public int UserId { get; set; }
    }
}
