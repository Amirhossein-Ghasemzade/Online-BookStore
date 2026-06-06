namespace BookStore.Models.DTOs;

public class BookDetail
{
    public int Id { get; init; }
    public string Title { get; init; } = null!;
    public string Slug { get; init; } = null!;
    public string? CategoryTitle { get; init; }
    public string Description { get; init; } = null!;
    public int PageCount { get; init; }
    public string? FilePath { get; init; }
    public List<AuthorInfo> Authors { get; init; } = [];
    public List<string> Keywords { get; init; } = [];
    public List<RelatedBook> RelatedBooks { get; set; } = [];
}
