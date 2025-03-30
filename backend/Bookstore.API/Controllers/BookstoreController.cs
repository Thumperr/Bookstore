using Bookstore.API.Data;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookstoreController : ControllerBase
{
    private BookstoreDbContext _bookstoreContext;

    public BookstoreController(BookstoreDbContext bookstoreContext)
    {
        _bookstoreContext = bookstoreContext;
    }

    /*public IActionResult GetBooks(int pageSize = 5, int pageNum = 1)
    {
        var something = _bookstoreContext.Books
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var totalNumBooks = _bookstoreContext.Books.Count();

        var someObject = new
        {
            Books = something,
            TotalNumBooks = totalNumBooks
        };
        
        return Ok(someObject);
    }*/
    
    [HttpGet("AllBooks")]
    public IActionResult GetBooks(int pageSize = 5, int pageNum = 1, string? sortOrder = null, [FromQuery] List<string>? bookCategories = null)
    {
        var booksQuery = _bookstoreContext.Books.AsQueryable();

        // Only apply sorting if sortOrder is provided
        if (!string.IsNullOrEmpty(sortOrder))
        {
            if (sortOrder.ToLower() == "desc")
            {
                booksQuery = booksQuery.OrderByDescending(b => b.Title);
            }
            else if (sortOrder.ToLower() == "asc")
            {
                booksQuery = booksQuery.OrderBy(b => b.Title);
            }
        }
        
        if (bookCategories != null && bookCategories.Any())
        {
            booksQuery = booksQuery.Where(b => bookCategories.Contains(b.Category));
        }
        
        var totalNumBooks = booksQuery.Count();

        var paginatedBooks = booksQuery
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var response = new
        {
            Books = paginatedBooks,
            TotalNumBooks = totalNumBooks
        };

        return Ok(response);
    }
    
    [HttpGet("GetBookCategories")]
    public IActionResult GetBookCategories()
    {
        var bookCategories = _bookstoreContext.Books
            .Select(b => b.Category)
            .Distinct()
            .ToList();
        
        return Ok(bookCategories);
    }
    
    [HttpPost("AddBook")]
    public IActionResult AddBook([FromBody] Book newBook)
    {
        _bookstoreContext.Books.Add(newBook);
        _bookstoreContext.SaveChanges();
        return Ok(newBook);
    }
    
    [HttpPut("UpdateBook/{bookId}")]
    public IActionResult UpdateBook(int bookId, [FromBody] Book updatedBook)
    {
        var existingBook = _bookstoreContext.Books.Find(bookId);

        existingBook.Title = updatedBook.Title;
        existingBook.Author = updatedBook.Author;
        existingBook.Publisher = updatedBook.Publisher;
        existingBook.ISBN = updatedBook.ISBN;
        existingBook.Classification = updatedBook.Classification;
        existingBook.Category = updatedBook.Category;
        existingBook.PageCount = updatedBook.PageCount;
        existingBook.Price = updatedBook.Price;
        
        _bookstoreContext.Books.Update(existingBook);
        _bookstoreContext.SaveChanges();

        return Ok(existingBook);
    }
    
    [HttpDelete("DeleteBook/{bookId}")]
    public IActionResult DeleteBook(int bookId)
    {
        var book = _bookstoreContext.Books.Find(bookId);

        if (book == null)
        {
            return NotFound(new { message = "Book not found" });
        }
        
        _bookstoreContext.Books.Remove(book);
        _bookstoreContext.SaveChanges();
        
        return NoContent();
    }
}