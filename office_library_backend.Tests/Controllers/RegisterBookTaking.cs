using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using Moq;
using NUnit.Framework;
using office_library_backend.Controllers;
using office_library_backend.Models;
using office_library_backend.Models.MyDto;
using System.Web.Http;
using System.Web.Http.Results;
using office_library_backend.Models.Repositories;

[TestFixture]
public class BookControllerTests
{
    private Mock<Entities2> _mockContext;
    private Mock<DbSet<AspNetUsers>> _mockUsersSet;
    private Mock<DbSet<Book>> _mockBooksSet;
    private Mock<DbSet<UserBookHistory>> _mockHistorySet;
    private Mock<IBaseRepository<UserBookHistoryDto, UserBookHistory>> _mockRepository;
    private UserBookHistoryController _controller;

    [SetUp]
    public void Setup()
    {
        _mockContext = new Mock<Entities2>();
        _mockUsersSet = CreateMockDbSet(new List<AspNetUsers>());
        _mockBooksSet = CreateMockDbSet(new List<Book>());
        _mockHistorySet = CreateMockDbSet(new List<UserBookHistory>());
        _mockRepository = new Mock<IBaseRepository<UserBookHistoryDto, UserBookHistory>>();

        _mockContext.Setup(m => m.AspNetUsers).Returns(_mockUsersSet.Object);
        _mockContext.Setup(m => m.Book).Returns(_mockBooksSet.Object);
        _mockContext.Setup(m => m.UserBookHistory).Returns(_mockHistorySet.Object);

        _controller = new UserBookHistoryController
        {
            dbContext = _mockContext.Object,
            repository = _mockRepository.Object
        };
    }

    private Mock<DbSet<T>> CreateMockDbSet<T>(List<T> data) where T : class
    {
        var queryable = data.AsQueryable();
        var mockSet = new Mock<DbSet<T>>();

        mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

        mockSet.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSet.Object);
        mockSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns((object[] ids) => data.FirstOrDefault());

        return mockSet;
    }

    [Test]
    public void RegisterBookTaking_WithValidData_ReturnsOk()
    {
        // Arrange
        var bookDto = new BooksDto
        {
            Id = "1",
            CurrentReader = new UsersDto { Id = "1" },
            IsTaken = false
        };

        var books = new List<Book> { new Book { Id = "1" } };
        var users = new List<AspNetUsers> { new AspNetUsers { Id = "1" } };

        _mockBooksSet = CreateMockDbSet(books);
        _mockUsersSet = CreateMockDbSet(users);

        _mockContext.Setup(m => m.Book).Returns(_mockBooksSet.Object);
        _mockContext.Setup(m => m.AspNetUsers).Returns(_mockUsersSet.Object);

        _mockRepository.Setup(m => m.Add(It.IsAny<UserBookHistory>(), _mockContext.Object));

        // Act
        IHttpActionResult result = _controller.RegisterBookTaking(bookDto);

        // Assert
        Assert.That(result, Is.InstanceOf<OkResult>());
        _mockRepository.Verify(m => m.Add(It.IsAny<UserBookHistory>(), _mockContext.Object), Times.Once);
        _mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }

    [Test]
    public void RegisterBookTaking_WhenReaderIsNull_ReturnsBadRequest()
    {
        // Arrange
        var bookDto = new BooksDto { CurrentReader = null };

        // Act
        IHttpActionResult result = _controller.RegisterBookTaking(bookDto);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestErrorMessageResult>());
        Assert.That(((BadRequestErrorMessageResult)result).Message,
                  Is.EqualTo("Книга или читатель не дошли до сервера"));
    }
}