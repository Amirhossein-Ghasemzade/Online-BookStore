using BookStore.Data;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class AuthorsController : Controller
{
    private readonly AppDbContext _context;

    public AuthorsController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        return View(await _context.Authors.AsNoTracking().OrderBy(a => a.FullName).ToListAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Create(string fullName, string slug)
    {
        _context.Authors.Add(new Author { FullName = fullName, Slug = slug });
        await _context.SaveChangesAsync();
        TempData["Success"] = "Author created.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, string fullName, string slug)
    {
        var author = await _context.Authors.FindAsync(id);
        if (author == null) return NotFound();
        author.FullName = fullName;
        author.Slug = slug;
        await _context.SaveChangesAsync();
        TempData["Success"] = "Author updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var author = await _context.Authors.FindAsync(id);
        if (author != null) { _context.Authors.Remove(author); await _context.SaveChangesAsync(); }
        TempData["Success"] = "Author deleted.";
        return RedirectToAction(nameof(Index));
    }
}
