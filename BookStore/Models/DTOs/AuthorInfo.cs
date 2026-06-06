namespace BookStore.Models.DTOs;

public record AuthorInfo(
    int Id,
    string FullName,
    string Slug
);
