namespace GraphQLBooks;

public class DataStore
{
    private readonly List<Book> _books = new();
    private readonly List<Author> _authors = new();

    public List<Book> Books => _books;
    public List<Author> Authors => _authors;

    public DataStore()
    {
        var author1 = new Author { Id = "1", Name = "J.K. Rowling" };
        var author2 = new Author { Id = "2", Name = "George R.R. Martin" };

        _authors.Add(author1);
        _authors.Add(author2);

        _books.Add(new Book
        {
            Id = "1", Title = "Harry Potter and the Philosopher's Stone", ReleaseYear = 1997, AuthorId = author1.Id,
            Author = author1
        });
        _books.Add(new Book
        {
            Id = "2", Title = "Harry Potter and the Chamber of Secrets", ReleaseYear = 1998, AuthorId = author1.Id,
            Author = author1
        });
        _books.Add(new Book
            { Id = "3", Title = "A Game of Thrones", ReleaseYear = 1996, AuthorId = author2.Id, Author = author2 });
    }
}