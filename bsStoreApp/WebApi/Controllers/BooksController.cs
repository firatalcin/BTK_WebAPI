using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;
using Repositories.EFCore;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IRepositoryManager _manager;

        public BooksController(IRepositoryManager manager)
        {
            _manager = manager; 
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            try
            {
                var books = _manager.Book.GetAllBooks(false);
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
                var book = _manager.Book.GetOneBookById(id, false);
                
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
                
                _manager.Book.CreateOneBook(book);
                _manager.Save();
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
                var entity = _manager.Book.GetOneBookById(id, true);
                
                if(entity is null)
                    return NotFound();
                
                if(id != book.Id)
                    return BadRequest();    
                
                entity.Title = book.Title;
                entity.Price = book.Price;
                _manager.Save();
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
                var entity = _manager.Book.GetOneBookById(id, true);

                if (entity is null)
                {
                    return NotFound(new
                    {
                        statusCode = 404,
                        message = $"Book with id: {id} not found"
                    });
                }
                
                _manager.Book.DeleteOneBook(entity);
                _manager.Save();
                return Ok(entity);
            }
            catch (Exception e)
            {
                
                throw new Exception(e.Message);
            }
        }
    }
}
