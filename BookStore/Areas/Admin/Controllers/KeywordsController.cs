using BookStore.Data;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class KeywordsController : Controller
{
    private readonly AppDbContext _context;

    public KeywordsController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        return View(await _context.Keywords.AsNoTracking().OrderBy(k => k.Word).ToListAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Create(string word)
    {
        _context.Keywords.Add(new Keyword { Word = word });
        await _context.SaveChangesAsync();
        TempData["Success"] = "Keyword created.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, string word)
    {
        var kw = await _context.Keywords.FindAsync(id);
        if (kw == null) return NotFound();
        kw.Word = word;
        await _context.SaveChangesAsync();
        TempData["Success"] = "Keyword updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var kw = await _context.Keywords.FindAsync(id);
        if (kw != null) { _context.Keywords.Remove(kw); await _context.SaveChangesAsync(); }
        TempData["Success"] = "Keyword deleted.";
        return RedirectToAction(nameof(Index));
    }
}
