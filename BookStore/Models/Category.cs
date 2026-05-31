namespace BookStore.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }

        public List<Book> Books { get; set; } = new();
    }
}
