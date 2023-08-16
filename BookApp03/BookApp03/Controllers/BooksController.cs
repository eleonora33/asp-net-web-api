using BookApp03.Models;
using Microsoft.AspNetCore.Mvc;


namespace BookApp03.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        [HttpGet]
        public ActionResult <List<Book>> GetAllBooks()
        {
            try
            {
                return Ok(StaticDb.Books);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{index}")]
        public ActionResult<Book> GetBookByIndex([FromRoute] int index)
        {
            try
            {
                if(index < 0)
                {
                    return BadRequest("The index cannot be negative!");
                }
                if(index >= StaticDb.Books.Count)
                {
                    return NotFound($"There is no resource on id {index}");
                }
                return Ok(StaticDb.Books[index]);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("queryString")]
        public ActionResult GetBookById([FromQuery] int? index)
        {
            try
            {
                if (index == null)
                {
                    return BadRequest("Index is a required parameter!");
                }
                if (index < 0)
                {
                    return BadRequest("The Index cannot be negative");
                }
                if (index >= StaticDb.Books.Count)
                {
                    return NotFound($"There is no resource on index {index}");
                }
                return Ok(StaticDb.Books[index.Value]);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("multipleQuery")]

        public ActionResult<List<Book>> FilterBooksWithQueryParams([FromQuery]string? text, int? id)
        {
            try
            {
                if (string.IsNullOrEmpty(text) && id == null)
                {
                    return BadRequest("You need to send at least one filter parameter!");
                }
                if(string.IsNullOrEmpty(text))
                {
                    List<Book> filteredBooksById = StaticDb.Books.Where(x => x.Id == id).ToList();
                    return  Ok(filteredBooksById);
                }
                if(id == null)
                {
                    List<Book> filteredBooksByText = StaticDb.Books.Where(t => t.Title.ToLower().Contains(text.ToLower())).ToList();
                    return Ok(filteredBooksByText);
                }
                List<Book> filteredBooks =
                    StaticDb.Books.Where(t => t.Title.ToLower().Contains(text.ToLower()) 
                        && t.Id == id).ToList();
                return Ok(filteredBooks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("Create")]
        public IActionResult CreatePostBook([FromBody] Book book)
        {
            try
            {
                if (string.IsNullOrEmpty(book.Title))
                {
                    return BadRequest("Each note must contain text!");
                }
                if (string.IsNullOrEmpty(book.Author))
                {
                    return BadRequest("Each note must contain text!");
                }
                StaticDb.Books.Add(book);
                return StatusCode(StatusCodes.Status201Created, "Book is created");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("ReturnTitles")]
        public IActionResult ReturnTitlesAsList([FromBody] List<Book> bookList)
        {
            try
            {
                if (bookList == null || bookList.Count == 0)
                {
                    return BadRequest("No books provided in the request body.");
                }
                List<string> titles = new List<string>();
                foreach (var book in bookList)
                {
                    var matchingBook = StaticDb.Books.FirstOrDefault(b => b.Title == book.Title);
                    if(matchingBook != null)
                    {
                        titles.Add(matchingBook.Title);
                    }

                }
                return Ok(titles.ToArray());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
