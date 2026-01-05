using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BibliotekaAPI.Models
{
    public class LibraryEntities
    {
        public class Author
        {
            public int Id { get; set; }

            [Required]
            [JsonPropertyName("first_name")] 
            public string FirstName { get; set; } = string.Empty;

            [Required]
            [JsonPropertyName("last_name")] 
            public string LastName { get; set; } = string.Empty;

            [JsonIgnore]
            public List<Book> Books { get; set; } = new();
        }

        public class Book
        {
            public int Id { get; set; }

            [Required]
            public string Title { get; set; } = string.Empty;

            [Range(0, int.MaxValue, ErrorMessage = "Year cannot be negative")]
            public int Year { get; set; }

            public int AuthorId { get; set; }

            public Author? Author { get; set; }

            [JsonIgnore]
            public List<BookCopy> Copies { get; set; } = new();
        }

        public class BookCopy
        {
            public int Id { get; set; }
            public int BookId { get; set; }
            public Book? Book { get; set; }
        }


        public class BookCreateDto
        {
            [Required]
            public string Title { get; set; } = string.Empty;
            [Range(0, int.MaxValue)]
            public int Year { get; set; }
            [Required]
            public int AuthorId { get; set; }
        }

        public class BookUpdateDto
        {
            public long Id { get; set; }

            [Required]
            public string Title { get; set; } = string.Empty;

            [Range(0, int.MaxValue)]
            public int Year { get; set; }

            [Required]
            public int AuthorId { get; set; }
        }
    }

}

