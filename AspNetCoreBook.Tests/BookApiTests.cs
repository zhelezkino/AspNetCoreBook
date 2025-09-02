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
            new() { Id = 1, Title = "Lukomorye", Author = "Pushkin", PublishedYear = 1828 },
            new() { Id = 2, Title = "War and peace", Author = "Tolstoy", PublishedYear = 1868 }
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
        Assert.Contains(books, b => b.Title == "Lukomorye");
        Assert.Contains(books, b => b.Author == "Pushkin");
    }

    [Fact]
    public async Task AddBook_SavesToDatabase()
    {
        // Arrange
        using var context = await GetTestDbContext();
        var newBook = new Book
        {
            Title = "A hero of our time",
            Author = "Lermontov",
            PublishedYear = 1840
        };

        // Act
        context.Books.Add(newBook);
        await context.SaveChangesAsync();

        // Assert
        var savedBook = await context.Books.FindAsync(newBook.Id);
        Assert.NotNull(savedBook);
        Assert.Equal("A hero of our time", savedBook.Title);
        Assert.Equal("Lermontov", savedBook.Author);
        Assert.Equal(1840, savedBook.PublishedYear);
    }

    [Fact]
    public async Task UpdateBook_UpdatesExistingBook()
    {
        // Arrange
        using var context = await GetTestDbContext();
        var book = await context.Books.FirstAsync(b => b.Title == "Lukomorye");

        // Act
        book.Title = "Lukomorye (Updated)";
        await context.SaveChangesAsync();

        // Assert
        var updatedBook = await context.Books.FindAsync(book.Id);
        Assert.Equal("Lukomorye (Updated)", updatedBook?.Title);
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
