namespace BookStore.Models.ViewModels;

public class BookDetailViewModel
{
    public Book Book { get; set; } = null!;
    public List<Book> RelatedBooks { get; set; } = [];
}
