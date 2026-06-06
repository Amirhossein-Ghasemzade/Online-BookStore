using BookStore.Data;
using BookStore.Models;
using BookStore.Models.ViewModels;
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
            .Include(a => a.BookAuthors)
            .ToListAsync();
        return View(authors);
    }

    public async Task<IActionResult> Books(string slug)
    {
        var author = await _context.Authors
            .FirstOrDefaultAsync(a => a.Slug == slug);

        if (author == null) return NotFound();

        var books = await _context.BookAuthors
            .Include(ba => ba.Book).ThenInclude(b => b.Category)
            .Where(ba => ba.AuthorId == author.Id)
            .Select(ba => ba.Book)
            .ToListAsync();

        var vm = new AuthorBooksViewModel
        {
            Author = author,
            Books = books
        };

        return View(vm);
    }
}
