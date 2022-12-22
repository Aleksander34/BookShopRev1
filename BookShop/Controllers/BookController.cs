using AutoMapper;
using BookShop.Core;
using BookShop.Core.Models;
using BookShop.Dto;
using BookShop.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BookShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : BookShopController
    {
        private readonly BookShopContext _context;
        private readonly IMapper _mapper;
        public BookController(BookShopContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpPost("[action]")]
        public IActionResult GetAll(GetAllBookDto input)
        {
            IQueryable<Book> querry = _context.Books;

            if (!string.IsNullOrWhiteSpace(input.BookTitle))
            {
                querry = querry.Where(x => x.Title.ToLower().Contains(input.BookTitle.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(input.AuthorName))
            {
                querry = querry.Where(x => x.BookAuthors.Any(y=>y.Author.Name.ToLower().Contains(input.AuthorName.ToLower())));
            }
            if (input.PublishedDateStart.HasValue)
            {
                if (input.PublishedDateEnd.HasValue)
                {
                    querry = querry.Where(x => x.PublishedOn <= input.PublishedDateEnd && x.PublishedOn>=input.PublishedDateStart);

                }
                else
                {
                    querry = querry.Where(x => x.PublishedOn == input.PublishedDateStart);
                }
            }
            if (input.PriceStart.HasValue )
            {
                if (input.PriceEnd.HasValue)
                {
                    querry = querry.Where(x => x.Price <= input.PriceEnd && x.Price>=input.PriceStart);
                }
                else
                {
                    querry = querry.Where(x => x.Price >= input.PriceStart);
                }
            }

            var totalCount = querry.Count();

            var books = querry
                .Include(x=>x.BookAuthors)
                .ThenInclude(y=>y.Author)
                .Include(p=>p.Property)
                .Include(t=>t.Reviews)
                .Skip(input.SkipCount)
                .Take(input.CountOnPage)
                .ToList();

            var result = _mapper.Map<IEnumerable<BookDto>>(books);

            return Ok(new { data = result,recordsTotal=totalCount,recordsFiltered = totalCount});
        }

        [HttpPost("[action]")]
        public IActionResult Create(BookDto input)
        {
            var book = _mapper.Map<Book>(input);
            _context.Books.Add(book);
            _context.SaveChanges();

            return Ok($"book {book.Id} added");
        }
        [HttpPost("[action]")]
        public IActionResult Remove(int id)
        {
            var book = _context.Books.Find(id);
            _context.Books.Remove(book);
            _context.SaveChanges();
            return Ok($"Book {book.Id} удален ");
        }
        [HttpPost("[action]")]
        public IActionResult Update(BookDto input)
        {
            var book = _mapper.Map<Book>(input);
            _context.Books.Update(book);
            _context.SaveChanges();
            return Ok($"Book {book.Id} updated");
        }

        [HttpPost("[action]")]
        public IActionResult GetPreviewBooks([FromForm]IFormFile input)
        {
            string pathToFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (!System.IO.Directory.Exists(Path.Combine(pathToFolder, "TemporaryStorage")))
            {
                Directory.CreateDirectory(Path.Combine(pathToFolder, "TemporaryStorage"));
            }

            string pathToFile = Path.Combine(pathToFolder, "TemporaryStorage", input.FileName);
            using (var stream = System.IO.File.Create(pathToFile))
            {
                input.CopyTo(stream);
            }
            var result = ExcelParcerUtil.ParseBook(pathToFile);
            System.IO.File.Delete(pathToFile);
            return Ok(result);
        }
    }
}
