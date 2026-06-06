namespace BookStore.Models.ViewModels.Admin;

public class AdminBookViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int PageCount { get; set; }
    public int CategoryId { get; set; }
    public string? FilePath { get; set; }
    public List<int> AuthorIds { get; set; } = [];
    public List<int> KeywordIds { get; set; } = [];
    public List<int> RelatedBookIds { get; set; } = [];
    public List<Category> AllCategories { get; set; } = [];
    public List<Author> AllAuthors { get; set; } = [];
    public List<Keyword> AllKeywords { get; set; } = [];
    public List<BookItem> AllBooks { get; set; } = [];
}

public record BookItem(int Id, string Title);
