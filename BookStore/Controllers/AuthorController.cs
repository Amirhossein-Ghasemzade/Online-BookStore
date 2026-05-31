using BookStore.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers;

[Authorize]
public class AuthorController : Controller
{
    private readonly AppDbContext _context;

    public AuthorController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var authors = await _context.Authors
            .Include(a => a.BookAuthors).ThenInclude(ba => ba.Book)
            .ToListAsync();
        return View(authors);
    }

    public async Task<IActionResult> Books(string slug)
    {
        var author = await _context.Authors
            .Include(a => a.BookAuthors).ThenInclude(ba => ba.Book).ThenInclude(b => b.Category)
            .FirstOrDefaultAsync(a => a.Slug == slug);

        if (author == null) return NotFound();

        return View(author);
    }
}
