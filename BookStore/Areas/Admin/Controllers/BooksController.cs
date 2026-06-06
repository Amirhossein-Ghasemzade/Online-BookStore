using BookStore.Data;
using BookStore.Models;
using BookStore.Models.ViewModels.Admin;
using BookStore.Services;
using BookStore.Services.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class BooksController : Controller
{
    private readonly AdminService _admin;

    public BooksController(AdminService admin)
    {
        _admin = admin;
    }

    public async Task<IActionResult> Index(string? search, int page = 1)
    {
        var vm = await _admin.GetBooksListAsync(search, page);
        return View(vm);
    }

    public async Task<IActionResult> Create()
    {
        var vm = await _admin.GetEmptyBookFormAsync();
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Create(AdminBookViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            await _admin.PopulateLookupsAsync(vm);
            return View(vm);
        }

        var slug = await _admin.CreateBookAsync(vm);
        TempData["Success"] = "Book created successfully.";
        return RedirectToAction(nameof(Edit), new { slug });
    }

    public async Task<IActionResult> Edit(string slug)
    {
        var vm = await _admin.GetBookFormAsync(slug);
        if (vm == null) return NotFound();
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(AdminBookViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            await _admin.PopulateLookupsAsync(vm);
            return View(vm);
        }

        await _admin.UpdateBookAsync(vm);
        TempData["Success"] = "Book updated successfully.";
        return RedirectToAction(nameof(Edit), new { slug = vm.Slug });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _admin.DeleteBookAsync(id);
        TempData["Success"] = "Book deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}
