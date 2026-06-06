using BookStore.Models.DTOs;

namespace BookStore.Models.ViewModels;

public class AuthorBooksViewModel
{
    public AuthorInfo Author { get; set; } = null!;
    public List<BookCardSimple> Books { get; set; } = [];
}
