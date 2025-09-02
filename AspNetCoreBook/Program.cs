////////////////////////////////////////////////////////////////////////////////
// See install and settings help in local file: AspNetCoreBook/Help/InitCommands.cs
////////////////////////////////////////////////////////////////////////////////

using AspNetCoreBook.Data;
using AspNetCoreBook.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;

////////////////////////////////////////////////////////////////////////////////
// Init builder
////////////////////////////////////////////////////////////////////////////////

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Добавляем DbContext с PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

////////////////////////////////////////////////////////////////////////////////
// Init app
////////////////////////////////////////////////////////////////////////////////

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else 
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

// Код, чтобы миграции применялись автоматически при старте
/*
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate(); // Применяет миграции
}
*/

////////////////////////////////////////////////////////////////////////////////
// Init minimal APIs для книг
////////////////////////////////////////////////////////////////////////////////

app.MapGet("/books", async (AppDbContext db) =>
    await db.Books.ToListAsync())
    .WithName("GetBooks")
    .WithOpenApi();

app.MapGet("/books/{id}", async (int id, AppDbContext db) =>
    await db.Books.FindAsync(id)
        is Book book
            ? Results.Ok(book)
            : Results.NotFound())
    .WithName("GetBook")
    .WithOpenApi();

app.MapPost("/books", async (Book book, AppDbContext db) =>
{
    db.Books.Add(book);
    await db.SaveChangesAsync();
    return Results.Created($"/books/{book.Id}", book);
})
.WithName("CreateBook")
.WithOpenApi();

app.MapPut("/books/{id}", async (int id, Book inputBook, AppDbContext db) =>
{
    var book = await db.Books.FindAsync(id);
    if (book is null) return Results.NotFound();

    book.Title = inputBook.Title;
    book.Author = inputBook.Author;
    book.PublishedYear = inputBook.PublishedYear;

    await db.SaveChangesAsync();
    return Results.NoContent();
})
.WithName("UpdateBook")
.WithOpenApi();

app.MapDelete("/books/{id}", async (int id, AppDbContext db) =>
{
    if (await db.Books.FindAsync(id) is Book book)
    {
        db.Books.Remove(book);
        await db.SaveChangesAsync();
        return Results.Ok();
    }

    return Results.NotFound();
})
.WithName("DeleteBook")
.WithOpenApi();

////////////////////////////////////////////////////////////////////////////////
// App run
////////////////////////////////////////////////////////////////////////////////

app.Run();
