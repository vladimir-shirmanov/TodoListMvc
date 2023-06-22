using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using TodoList.Data;
using TodoList.Models;
using TodoList.Services;

namespace TodoList.Tests
{
    [TestFixture]
    public class TodoItemServiceTests
    {
        private readonly Mock<ILogger<TodoItemService>> _loggerMock;
        private readonly Mock<TodoContext> _contextMock;

        private TodoItemService _todoItemService;

        public TodoItemServiceTests()
        {
            _loggerMock = new Mock<ILogger<TodoItemService>>();
            _contextMock = new Mock<TodoContext>();
        }


        [SetUp]
        public void Setup()
        {
            _todoItemService = new TodoItemService(_contextMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task GetTodoItem_ShouldReturnValue_IfExistsInDb()
        {
            // Arrange
            var todoItems = new List<TodoItem> {new TodoItem
            {
                Id = 1,
                Title = "Test Todo Item",
                Description = "Test Todo Item Description",
                Category = new Category
                {
                    Description = "Test Category Description",
                    Title = "Test Category",
                },
            }
            }.AsQueryable();
            Mock<DbSet<TodoItem>> _mockSet = new Mock<DbSet<TodoItem>>();
            _mockSet.As<IQueryable>().Setup(m => m.Provider).Returns(todoItems.Provider);
            _mockSet.As<IQueryable>().Setup(m => m.Expression).Returns(todoItems.Expression);
            _mockSet.As<IQueryable>().Setup(m => m.ElementType).Returns(todoItems.ElementType);
            _mockSet.As<IQueryable>().Setup(m => m.GetEnumerator()).Returns(todoItems.GetEnumerator());
            _contextMock.Setup(x => x.TodoItems).Returns(_mockSet.Object);

            // Act
            var result = await _todoItemService.GetTodoItemAsync(1);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetTodoItem_ShouldReturnNull_IfNotExistsInDb()
        {
            // Arrange
            _contextMock.Setup(x => x.TodoItems.FindAsync(1)).ReturnsAsync((TodoItem)null);

            // Act
            var result = await _todoItemService.GetTodoItemAsync(1);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetTodoItem_ShouldLogErrorAndReturnNull_IfDbThrowsException()
        {
            // Arrange
            _contextMock.Setup(x => x.TodoItems.FindAsync(1)).ThrowsAsync(new Exception("Test Exception"));

            // Act
            var result = await _todoItemService.GetTodoItemAsync(1);

            // Assert
            Assert.IsNull(result);
            _loggerMock.Verify(x => x.Log(
                            It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                            It.IsAny<EventId>(),
                            It.Is<It.IsAnyType>((@object, @type) => true),
                            It.IsAny<Exception>(),
                            It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                    Times.Once);
        }

        [Test]
        public async Task GetTodoItem_ShouldLogInformation_IfTodoItemExistsInDb()
        {
            // Arrange
            var todoItems = new List<TodoItem> {new TodoItem
            {
                Id = 1,
                Title = "Test Todo Item",
                Description = "Test Todo Item Description",
                Category = new Category
                {
                    Description = "Test Category Description",
                    Title = "Test Category",
                },
            }
            }.AsQueryable();
            Mock<DbSet<TodoItem>> _mockSet = new Mock<DbSet<TodoItem>>();
            _mockSet.As<IQueryable>().Setup(m => m.Provider).Returns(todoItems.Provider);
            _mockSet.As<IQueryable>().Setup(m => m.Expression).Returns(todoItems.Expression);
            _mockSet.As<IQueryable>().Setup(m => m.ElementType).Returns(todoItems.ElementType);
            _mockSet.As<IQueryable>().Setup(m => m.GetEnumerator()).Returns(todoItems.GetEnumerator());

            // Act
            var result = await _todoItemService.GetTodoItemAsync(1);

            // Assert
            _loggerMock.Verify(x => x.Log(
                                           It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                                           It.IsAny<EventId>(),
                                           It.Is<It.IsAnyType>((@object, @type) => true),
                                           It.IsAny<Exception>(),
                                           It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                               Times.Once);
        }

        [Test]
        public async Task GetTodoItem_ShouldLogInformation_IfTodoItemNotExistsInDb()
        {
            // Arrange
            _contextMock.Setup(x => x.TodoItems.FindAsync(1)).ReturnsAsync((TodoItem)null);

            // Act
            var result = await _todoItemService.GetTodoItemAsync(1);

            // Assert
            _loggerMock.Verify(x => x.Log(
                                          It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                                          It.IsAny<EventId>(),
                                          It.Is<It.IsAnyType>((@object, @type) => true),
                                          It.IsAny<Exception>(),
                                          It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                              Times.Once);
        }

        [Test]
        public async Task GetTodoItems_ShouldReturnAllTodoItems()
        {
            // Arrange
            var todoItems = new List<TodoItem>
            {
                new TodoItem
                {
                    Id = 1,
                    Title = "Test Todo Item 1",
                    Description = "Test Todo Item Description 1",
                    Category = new Category
                    {
                        Description = "Test Category Description 1",
                        Title = "Test Category 1",
                    },
                },
                new TodoItem
                {
                    Id = 2,
                    Title = "Test Todo Item 2",
                    Description = "Test Todo Item Description 2",
                    Category = new Category
                    {
                        Description = "Test Category Description 2",
                        Title = "Test Category 2",
                    },
                },
            }.AsQueryable();
            Mock<DbSet<TodoItem>> _mockSet = new Mock<DbSet<TodoItem>>();
            _mockSet.As<IQueryable>().Setup(m => m.Provider).Returns(todoItems.Provider);
            _mockSet.As<IQueryable>().Setup(m => m.Expression).Returns(todoItems.Expression);
            _mockSet.As<IQueryable>().Setup(m => m.ElementType).Returns(todoItems.ElementType);
            _mockSet.As<IQueryable>().Setup(m => m.GetEnumerator()).Returns(todoItems.GetEnumerator());
            _contextMock.Setup(x => x.TodoItems).Returns(_mockSet.Object);

            // Act
            var result = await _todoItemService.GetTodoItemsAsync();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetTodoItems_ShouldLogErrorAndReturnNull_IfDbThrowsException()
        {
            // Arrange
            _contextMock.Setup(x => x.TodoItems).Throws(new Exception("Test Exception"));

            // Act
            var result = await _todoItemService.GetTodoItemsAsync();

            // Assert
            Assert.IsNull(result);
            _loggerMock.Verify(x => x.Log(
                                           It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                                           It.IsAny<EventId>(),
                                           It.Is<It.IsAnyType>((@object, @type) => true),
                                           It.IsAny<Exception>(),
                                           It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                                   Times.Once);
        }

        [Test]
        public async Task GetTodoItems_ShouldLogInformation_IfTodoItemsExistsInDb()
        {
            // Arrange
            var todoItems = new List<TodoItem>
            {
                new TodoItem
                {
                    Id = 1,
                    Title = "Test Todo Item 1",
                    Description = "Test Todo Item Description 1",
                    Category = new Category
                    {
                        Description = "Test Category Description 1",
                        Title = "Test Category 1",
                    },
                },
                new TodoItem
                {
                    Id = 2,
                    Title = "Test Todo Item 2",
                    Description = "Test Todo Item Description 2",
                    Category = new Category
                    {
                        Description = "Test Category Description 2",
                        Title = "Test Category 2",
                    },
                },
            }.AsQueryable();
            Mock<DbSet<TodoItem>> _mockSet = new Mock<DbSet<TodoItem>>();
            _mockSet.As<IQueryable>().Setup(m => m.Provider).Returns(todoItems.Provider);
            _mockSet.As<IQueryable>().Setup(m => m.Expression).Returns(todoItems.Expression);
            _mockSet.As<IQueryable>().Setup(m => m.ElementType).Returns(todoItems.ElementType);
            _mockSet.As<IQueryable>().Setup(m => m.GetEnumerator()).Returns(todoItems.GetEnumerator());
            _contextMock.Setup(x => x.TodoItems).Returns(_mockSet.Object);

            // Act
            var result = await _todoItemService.GetTodoItemsAsync();

            // Assert
            _loggerMock.Verify(x => x.Log(
                                           It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                                           It.IsAny<EventId>(),
                                           It.Is<It.IsAnyType>((@object, @type) => true),
                                           It.IsAny<Exception>(),
                                           It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                                      Times.Once);
        }

        [Test]
        public async Task GetTodoItems_ShouldLogInformation_IfTodoItemsNotExistsInDb()
        {
            // Arrange
            var todoItems = new List<TodoItem>().AsQueryable();
            Mock<DbSet<TodoItem>> _mockSet = new Mock<DbSet<TodoItem>>();
            _mockSet.As<IQueryable>().Setup(m => m.Provider).Returns(todoItems.Provider);
            _mockSet.As<IQueryable>().Setup(m => m.Expression).Returns(todoItems.Expression);
            _mockSet.As<IQueryable>().Setup(m => m.ElementType).Returns(todoItems.ElementType);
            _mockSet.As<IQueryable>().Setup(m => m.GetEnumerator()).Returns(todoItems.GetEnumerator());
            _contextMock.Setup(x => x.TodoItems).Returns(_mockSet.Object);

            // Act
            var result = await _todoItemService.GetTodoItemsAsync();

            // Assert
            _loggerMock.Verify(x => x.Log(
                                           It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                                           It.IsAny<EventId>(),
                                           It.Is<It.IsAnyType>((@object, @type) => true),
                                           It.IsAny<Exception>(),
                                           It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                                     Times.Once);
        }

        [Test]
        public async Task GetTodoItems_ShouldReturnNull_IfTodoItemsNotExistsInDb()
        {
            // Arrange
            var todoItems = new List<TodoItem>().AsQueryable();
            Mock<DbSet<TodoItem>> _mockSet = new Mock<DbSet<TodoItem>>();
            _mockSet.As<IQueryable>().Setup(m => m.Provider).Returns(todoItems.Provider);
            _mockSet.As<IQueryable>().Setup(m => m.Expression).Returns(todoItems.Expression);
            _mockSet.As<IQueryable>().Setup(m => m.ElementType).Returns(todoItems.ElementType);
            _mockSet.As<IQueryable>().Setup(m => m.GetEnumerator()).Returns(todoItems.GetEnumerator());
            _contextMock.Setup(x => x.TodoItems).Returns(_mockSet.Object);

            // Act
            var result = await _todoItemService.GetTodoItemsAsync();

            // Assert
            Assert.IsNull(result);
        }
    }
}
