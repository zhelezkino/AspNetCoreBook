using AspNetCoreBook.Data;
using AspNetCoreBook.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCoreBook.Tests;

public class BookApiTests
{
    private async Task<AppDbContext> GetTestDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: $"test_db_{Guid.NewGuid()}")
            .Options;

        var context = new AppDbContext(options);
        context.Database.EnsureCreated();

        // Добавим тестовые данные
        context.Books.AddRange(new List<Book>
        {
            new() { Id = 1, Title = "1984", Author = "George Orwell", PublishedYear = 1949 },
            new() { Id = 2, Title = "Fahrenheit 451", Author = "Ray Bradbury", PublishedYear = 1953 }
        });
        await context.SaveChangesAsync();

        return context;
    }

    [Fact]
    public async Task GetBooks_ReturnsAllBooks()
    {
        // Arrange
        using var context = await GetTestDbContext();

        // Act
        var books = await context.Books.ToListAsync();

        // Assert
        Assert.Equal(2, books.Count);
        Assert.Contains(books, b => b.Title == "1984");
        Assert.Contains(books, b => b.Author == "Ray Bradbury");
    }

    [Fact]
    public async Task AddBook_SavesToDatabase()
    {
        // Arrange
        using var context = await GetTestDbContext();
        var newBook = new Book
        {
            Title = "The Hobbit",
            Author = "J.R.R. Tolkien",
            PublishedYear = 1937
        };

        // Act
        context.Books.Add(newBook);
        await context.SaveChangesAsync();

        // Assert
        var savedBook = await context.Books.FindAsync(newBook.Id);
        Assert.NotNull(savedBook);
        Assert.Equal("The Hobbit", savedBook.Title);
        Assert.Equal("J.R.R. Tolkien", savedBook.Author);
    }

    [Fact]
    public async Task UpdateBook_UpdatesExistingBook()
    {
        // Arrange
        using var context = await GetTestDbContext();
        var book = await context.Books.FirstAsync(b => b.Title == "1984");

        // Act
        book.Title = "1984 (Updated)";
        await context.SaveChangesAsync();

        // Assert
        var updatedBook = await context.Books.FindAsync(book.Id);
        Assert.Equal("1984 (Updated)", updatedBook?.Title);
    }

    [Fact]
    public async Task DeleteBook_RemovesFromDatabase()
    {
        // Arrange
        using var context = await GetTestDbContext();
        var book = await context.Books.FirstAsync();

        // Act
        context.Books.Remove(book);
        await context.SaveChangesAsync();

        // Assert
        var exists = await context.Books.AnyAsync(b => b.Id == book.Id);
        Assert.False(exists);
    }
}
