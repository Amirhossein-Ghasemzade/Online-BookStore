namespace BookStore.Models.ViewModels.Admin;

public class PaginatedList<T>
{
    public List<T> Items { get; }
    public int Page { get; }
    public int TotalPages { get; }
    public bool HasPrev => Page > 1;
    public bool HasNext => Page < TotalPages;

    public PaginatedList(List<T> items, int total, int page, int pageSize)
    {
        Items = items;
        Page = page;
        TotalPages = (int)Math.Ceiling(total / (double)pageSize);
    }
}
