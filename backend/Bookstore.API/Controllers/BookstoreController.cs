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

    public IEnumerable<Book> GetBooks()
    {
        return _bookstoreContext.Books.ToList();
    }
}