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
    
    public IActionResult GetBooks(int pageSize = 5, int pageNum = 1, string? sortOrder = null)
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

        var paginatedBooks = booksQuery
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var totalNumBooks = _bookstoreContext.Books.Count();

        var response = new
        {
            Books = paginatedBooks,
            TotalNumBooks = totalNumBooks
        };

        return Ok(response);
    }
}