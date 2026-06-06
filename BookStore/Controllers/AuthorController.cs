using BookStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers;

[Authorize]
public class AuthorController : Controller
{
    private readonly IAuthorService _authorService;

    public AuthorController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    public async Task<IActionResult> Index()
    {
        var authors = await _authorService.GetAuthorsAsync();
        return View(authors);
    }

    public async Task<IActionResult> Books(string slug)
    {
        var vm = await _authorService.GetAuthorBooksAsync(slug);
        if (vm == null) return NotFound();
        return View(vm);
    }
}
