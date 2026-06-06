using BookStore.Data;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class CategoriesController : Controller
{
    private readonly AppDbContext _context;

    public CategoriesController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        return View(await _context.Categories.AsNoTracking().OrderBy(c => c.Title).ToListAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Create(string title, string slug)
    {
        _context.Categories.Add(new Category { Title = title, Slug = slug });
        await _context.SaveChangesAsync();
        TempData["Success"] = "Category created.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, string title, string slug)
    {
        var cat = await _context.Categories.FindAsync(id);
        if (cat == null) return NotFound();
        cat.Title = title;
        cat.Slug = slug;
        await _context.SaveChangesAsync();
        TempData["Success"] = "Category updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var cat = await _context.Categories.FindAsync(id);
        if (cat != null) { _context.Categories.Remove(cat); await _context.SaveChangesAsync(); }
        TempData["Success"] = "Category deleted.";
        return RedirectToAction(nameof(Index));
    }
}
