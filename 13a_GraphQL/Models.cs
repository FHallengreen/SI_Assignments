namespace GraphQLBooks;

public class Book
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? Title { get; set; }
    public int? ReleaseYear { get; set; }
    public string AuthorId { get; set; } = string.Empty;
    public Author? Author { get; set; }
}

public class Author
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? Name { get; set; }
    public List<Book>? Books { get; set; }
}

public class ErrorMessage
{
    public string? Message { get; set; }
    public int? ErrorCode { get; set; }
}

public class SuccessMessage
{
    public string? Message { get; set; }
}