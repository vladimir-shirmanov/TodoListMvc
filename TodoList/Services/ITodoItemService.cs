using TodoList.Models;

namespace TodoList.Services
{
    public interface ITodoItemService
    {
        Task<List<TodoItem>?> GetTodoItemsAsync();
        Task<TodoItem?> GetTodoItemAsync(int id);
        Task<bool> CompleteTodoItemAsync(int id);
        Task<TodoItem?> SaveTodoItemAsync(TodoItem todoItem);
        Task<TodoItem?> UpdateTodoItemAsync(TodoItem todoItem);
    }
}