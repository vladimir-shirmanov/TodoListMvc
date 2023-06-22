using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using TodoList.Models;
using TodoList.Services;

namespace TodoList.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITodoItemService _todoItemService;

        public HomeController(ILogger<HomeController> logger, ITodoItemService todoItemService)
        {
            _logger = logger;
            _todoItemService = todoItemService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var todoItems = await _todoItemService.GetTodoItemsAsync();
            return View(todoItems);
        }

        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "Work", Value = "1" },
                new SelectListItem { Text = "Personal", Value = "2" },
                new SelectListItem { Text = "Family", Value = "3" },
                new SelectListItem { Text = "Private", Value = "4" },
            }, "Value", "Text");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TodoItem item)
        {
            if (ModelState.IsValid)
            {
                item.UserId = 1;

                await _todoItemService.SaveTodoItemAsync(item);
                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}