using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using WebApi.DBOperations;
using WebApi.BookOperations.GetBooks;
using WebApi.BookOperations.CreateBook;
using static WebApi.BookOperations.CreateBook.CreateBookCommand;

namespace WebApi.Controller
{
    [ApiController] 
    [Route("[controller]s")] 
    public class BookController: ControllerBase
    {
        private readonly BookStoreDbContext _context;
        public BookController(BookStoreDbContext context)
        {
            _context = context;
         }
        

        [HttpGet]
         public IActionResult GetBooks()
        {
           GetBooksQuery query = new GetBooksQuery(_context);
           var result = query.Handle();
           return Ok(result);
        }

        [HttpGet("{id}")]
        public Book GetByID(int id)
        {
            var book = _context.Books.Where(book=> book.Id == id).SingleOrDefault();
            return book; // endpoint
        }
        // [HttpGet]
        // public Book Get([FromQuery] string id)
        // {
        //     var book = BookList.Where(book=> book.Id == Convert.ToInt32(id)).SingleOrDefault();
        //     return book; // endpoint
        // }

        //post
        [HttpPost]
        public IActionResult AddBook([FromBody] CreateBookModel newBook)
        {
            
            CreateBookCommand command = new CreateBookCommand(_context);
            try{
                command.Model = newBook;
                command.Handle();

            }
            catch(Exception ex )
            {
                return BadRequest(ex.Message);
            }
            
          
            return Ok();
        }

        // put
        [HttpPut("{id}")]
       
        public IActionResult UpdateBook (int id, [FromBody] Book updatedBook)
        {
            var book = _context.Books.SingleOrDefault(x=> x.Id == id);
            if (book is null)
            
                return BadRequest();
            book.GenreId = updatedBook.GenreId != default  ? updatedBook.GenreId : book.GenreId;
            book.PageCount = updatedBook.PageCount != default ? updatedBook.PageCount : book.PageCount;
            book.PublishDate = updatedBook.PublishDate != default ? updatedBook.PublishDate : book.PublishDate;
            book.Title = updatedBook.Title != default ? updatedBook.Title : book.Title;    
            _context.SaveChanges(); 
            return Ok();
       }
       //delete
       [HttpDelete("{id}")]
       public IActionResult DeleteBook(int id)
       {
        var book = _context.Books.SingleOrDefault(x => x.Id == id);
        if(book is null)
            return BadRequest();
        _context.Books.Remove(book);
        _context.SaveChanges();
        return Ok();
       }
    }
}