namespace BookStore.Models.ViewModels;

public class AuthorBooksViewModel
{
    public Author Author { get; set; } = null!;
    public List<Book> Books { get; set; } = [];
}
