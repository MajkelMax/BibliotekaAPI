using BibliotekaAPI.Data;
using Microsoft.AspNetCore.Mvc;
using static BibliotekaAPI.Models.LibraryEntities;
using Microsoft.EntityFrameworkCore;

namespace BibliotekaAPI.Controllers
{
    [Route("books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryContext _context;

        public BooksController(LibraryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks([FromQuery] int? authorId)
        {
            var query = _context.Books.Include(b => b.Author).AsQueryable();

            if (authorId.HasValue)
            {
                query = query.Where(b => b.AuthorId == authorId.Value);
            }

            return await query.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(long id)
        {
            if (id > int.MaxValue)
            {
                return NotFound();
            }
            var book = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.Id == (int)id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(BookCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var author = await _context.Authors.FindAsync(dto.AuthorId);
            if (author == null) return BadRequest("Author does not exist");

            var book = new Book
            {
                Title = dto.Title,
                Year = dto.Year,
                AuthorId = dto.AuthorId,
                Author = author
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(long id, BookUpdateDto bookDto)
        {
            if (id > int.MaxValue)
            {
                return NotFound();
            }
            int bookId = (int)id;

            if (bookDto.Id != 0 && bookDto.Id != bookId)
            {
                return BadRequest("ID mismatch");
            }

            var authorExists = await _context.Authors.AnyAsync(a => a.Id == bookDto.AuthorId);
            if (!authorExists)
            {
                return BadRequest("Invalid Author ID");
            }

            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
            {
                return NotFound();
            }

            book.Title = bookDto.Title;
            book.Year = bookDto.Year;
            book.AuthorId = bookDto.AuthorId;

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Books.Any(e => e.Id == bookId)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(long id)
        {
            if (id > int.MaxValue)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync((int)id);

            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
