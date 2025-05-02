using HotChocolate.Types;

namespace GraphQLBooks;

public class Query
{
    public List<Book> Books([Service] DataStore data) => data.Books;

    public Book? Book(string id, [Service] DataStore data) =>
        data.Books.FirstOrDefault(b => b.Id == id);

    public List<Author> Authors([Service] DataStore data) => data.Authors;

    public Author? Author(string id, [Service] DataStore data) =>
        data.Authors.FirstOrDefault(a => a.Id == id);
}

public class Mutation
{
    public Book CreateBook(string authorId, string title, int? releaseYear,
        [Service] DataStore data)
    {
        var author = data.Authors.FirstOrDefault(a => a.Id == authorId);
        if (author == null)
            throw new GraphQLException("Author not found.");

        var book = new Book
        {
            Title = title,
            ReleaseYear = releaseYear,
            AuthorId = authorId,
            Author = author
        };
        data.Books.Add(book);
        return book;
    }

    public Book? UpdateBook(string id, string? authorId, string? title, int? releaseYear,
        [Service] DataStore data)
    {
        var book = data.Books.FirstOrDefault(b => b.Id == id);
        if (book == null)
            throw new GraphQLException("Book not found.");

        if (authorId != null)
        {
            var author = data.Authors.FirstOrDefault(a => a.Id == authorId);
            if (author == null)
                throw new GraphQLException("Author not found.");
            book.AuthorId = authorId;
            book.Author = author;
        }

        book.Title = title ?? book.Title;
        book.ReleaseYear = releaseYear ?? book.ReleaseYear;
        return book;
    }

    public SuccessMessage DeleteBook(string id, [Service] DataStore data)
    {
        var book = data.Books.FirstOrDefault(b => b.Id == id);
        if (book == null)
            throw new GraphQLException("Book not found.");

        data.Books.Remove(book);
        return new SuccessMessage { Message = "Book deleted successfully." };
    }
}

[ExtendObjectType("Book")]
public class BookResolvers
{
    public Author? Author([Parent] Book book, [Service] DataStore data) =>
        data.Authors.FirstOrDefault(a => a.Id == book.AuthorId);
}

[ExtendObjectType("Author")]
public class AuthorResolvers
{
    public List<Book>? Books([Parent] Author author, [Service] DataStore data) =>
        data.Books.Where(b => b.AuthorId == author.Id).ToList();
}