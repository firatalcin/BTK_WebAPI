using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.EFCore;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly RepositoryContext _context;

        public BooksController(RepositoryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            try
            {
                var books = _context.Books.ToList();
                return Ok(books);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
        {
            try
            {
                var book = _context.Books.Find(id);
                
                if(book is null)
                    return NotFound();
                
                return Ok(book);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateOneBook([FromBody] Book book)
        {
            try
            {
                if(string.IsNullOrEmpty(book.Title))
                    return BadRequest();
                
                _context.Books.Add(book);   
                _context.SaveChanges();
                return StatusCode(201, book);   
    
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id,[FromBody] Book book)
        {
            try
            {
                var enttiy = _context.Books.Where(x => x.Id == id).SingleOrDefault();
                
                if(enttiy is null)
                    return NotFound();
                
                if(id != book.Id)
                    return BadRequest();    
                
                enttiy.Title = book.Title;
                enttiy.Price = book.Price;
                _context.SaveChanges();
                return Ok(book);
                
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute(Name = "id")] int id)
        {
            try
            {
                var entity = _context.Books.Where(x => x.Id == id).SingleOrDefault();

                if (entity is null)
                {
                    return NotFound(new
                    {
                        statusCode = 404,
                        message = $"Book with id: {id} not found"
                    });
                }
                
                _context.Books.Remove(entity);
                _context.SaveChanges();
                return Ok(entity);
            }
            catch (Exception e)
            {
                
                throw new Exception(e.Message);
            }
        }
    }
}
