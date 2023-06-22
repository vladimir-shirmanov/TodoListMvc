using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TodoList.Data;
using TodoList.Models;

namespace TodoList.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly TodoContext _context;
        private readonly ILogger<TodoItemService> _logger;

        public TodoItemService(TodoContext context, ILogger<TodoItemService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> CompleteTodoItemAsync(int id)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            try
            {
                var item = await _context.TodoItems.FindAsync(id);
                if (item == null)
                {
                    _logger.LogWarning($"WARNING Completing TodoItem with id not found {{@Details}}", new { Id = id });
                    return false;
                }

                item.IsCompleted = true;
                _context.Entry(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                stopwatch.Stop();
                _logger.LogInformation($"END Completing TodoItem with id {{@Details}}", new { Id = id, stopwatch.ElapsedMilliseconds });
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ERROR Completing TodoItem with id {@Details}", new { Id = id, stopwatch.ElapsedMilliseconds });
                return false;
            }
        }

        public async Task<TodoItem?> GetTodoItemAsync(int id)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            try
            {
                _logger.LogDebug($"BEGIN Getting TodoItem with id {{@Details}}", new { Id = id });

                var todoItem = await _context.TodoItems.FindAsync(id);

                stopwatch.Stop();
                _logger.LogInformation($"END Getting TodoItem with id {{@Details}}", new { Id = id, stopwatch.ElapsedMilliseconds });

                return todoItem;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ERROR Getting TodoItem with id {@Details}", new { Id = id, stopwatch.ElapsedMilliseconds });
                return null;
            }
        }

        public async Task<List<TodoItem>?> GetTodoItemsAsync()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            try
            {
                _logger.LogDebug($"BEGIN Getting TodoItems");

                var todoItems = await _context.TodoItems.ToListAsync();

                stopwatch.Stop();
                _logger.LogInformation($"END Getting TodoItems", new { stopwatch.ElapsedMilliseconds });

                return todoItems;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ERROR Getting TodoItems", new { stopwatch.ElapsedMilliseconds });
                return null;
            }
        }

        public async Task<TodoItem?> SaveTodoItemAsync(TodoItem todoItem)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            try
            {
                _logger.LogDebug($"BEGIN Saving TodoItem {{@Details}}", new { todoItem });

                _context.TodoItems.Add(todoItem);
                await _context.SaveChangesAsync();

                stopwatch.Stop();
                _logger.LogInformation($"END Saving TodoItem {{@Details}}", new { todoItem, stopwatch.ElapsedMilliseconds });

                return todoItem;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ERROR Saving TodoItem {@Details}", new { todoItem, stopwatch.ElapsedMilliseconds });
                return null;
            }
        }

        public async Task<TodoItem?> UpdateTodoItemAsync(TodoItem todoItem)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            try
            {
                _logger.LogDebug($"BEGIN Updating TodoItem {{@Details}}", new { todoItem });

                _context.Entry(todoItem).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                stopwatch.Stop();
                _logger.LogInformation($"END Updating TodoItem {{@Details}}", new { todoItem, stopwatch.ElapsedMilliseconds });

                return todoItem;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ERROR Updating TodoItem {@Details}", new { todoItem, stopwatch.ElapsedMilliseconds });
                return null;
            }
        }
    }
}