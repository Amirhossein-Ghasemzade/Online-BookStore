using BookStore.Models.DTOs;

namespace BookStore.Models.ViewModels;

public class BookIndexViewModel
{
    public List<BookCard> Books { get; set; } = [];
    public List<Category> Categories { get; set; } = [];
    public List<Author> Authors { get; set; } = [];
    public string? CategorySlug { get; set; }
    public string? AuthorSlug { get; set; }
    public string? Keyword { get; set; }
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; } = 1;
}