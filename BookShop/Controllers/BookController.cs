using AutoMapper;
using BookShop.Core;
using BookShop.Core.Models;
using BookShop.Dto;
using BookShop.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BookShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    [Authorize]
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
        public async Task<IActionResult> GetAllAsync(GetAllBookDto input)
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
            if (DateTime.TryParse(input.PublishedDateStart, out DateTime dateStart))
            {
                if (DateTime.TryParse(input.PublishedDateEnd, out DateTime dateEnd))
                {
                    querry = querry.Where(x => x.PublishedOn <= dateEnd && x.PublishedOn >= dateStart);

                }
                else
                {
                    querry = querry.Where(x => x.PublishedOn == dateStart);
                }
            }
            if (input.PriceStart.HasValue)
            {
                if (input.PriceEnd.HasValue)
                {
                    querry = querry.Where(x => x.Price <= input.PriceEnd && x.Price >= input.PriceStart);
                }
                else
                {
                    querry = querry.Where(x => x.Price >= input.PriceStart);
                }
            }
            if (!string.IsNullOrWhiteSpace(input.Category))
            {
                querry = querry.Where(x => x.Category == input.Category);
            }
            if (!string.IsNullOrWhiteSpace(input.Search))
            {
                string search = input.Search.ToLower();
                querry = querry.Where(x => x.Category.ToLower().Contains(search) || x.Title.ToLower().Contains(search) || x.BookAuthors.Any(y => y.Author.Name.ToLower().Contains(search)));
            }


            var totalCount = querry.Count();

            var books = querry
                .Include(p => p.Property)
                .Include(t => t.Reviews)
                .Skip(input.SkipCount)
                .Take(input.CountOnPage)
                .AsNoTracking()
                .ToList();


            var result = _mapper.Map<IEnumerable<BookDto>>(books);

            foreach (var b in result)
            {
                b.Authors = string.Join(",", _context.BookAuthors
                    .Include(x => x.Author)
                    .Where(x => x.BookId == b.Id)
                    .AsNoTracking()
                    .Select(x => x.Author.Name)
                    .ToArray());
            };

            return Ok(new { data = result,recordsTotal=totalCount,recordsFiltered = totalCount});
        }

        [HttpGet("[action]")]
        public IActionResult Get(int id)
        {
            var books = _context.Books
                .Include(x => x.BookAuthors)
                .ThenInclude(y => y.Author)
                .Include(p => p.Property)
                .Include(t => t.Reviews)
                .First(x=>x.Id==id);

            var result = _mapper.Map<BookDto>(books);

            return Ok(result);
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
        public IActionResult Remove([FromQuery]int id)
        {
            var book = _context.Books.Find(id);
            if (book == null)
            {
                return BadRequest("Не найдена книга с таким ID");
            }
            _context.Books.Remove(book);
            _context.SaveChanges();
            return Ok($"Book {book.Id} удален ");
        }
        [HttpPost("[action]")]
        public IActionResult Update(BookDto input)
        {
            var book = _mapper.Map<Book>(input);
            var bookAuthors = _context.BookAuthors.Where(x => x.BookId == book.Id);
            foreach (var bookAuthor in bookAuthors)
            {
                _context.BookAuthors.Remove(bookAuthor);
            }

            var authors = input.Authors.Split(',');
            foreach (var authorName in authors)
            {
                var author = _context.Authors.FirstOrDefault(x => x.Name == authorName);
                if (author == null)
                {
                    author = new Author { Name = authorName };
                    _context.Authors.Add(author);
                    _context.SaveChanges();
                }

                _context.BookAuthors.Add(new BookAuthor
                {
                    BookId = book.Id,
                    AuthorId = author.Id,
                });
            }
            _context.Books.Update(book);
            _context.SaveChanges();
            return Ok($"Book {book.Id} updated");
        }

        [HttpPost("[action]")]
        public IActionResult AddBooks([FromForm]IFormFile input)
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

            foreach (var bookDto in result.Books)
            {
                var book = _mapper.Map<Book>(bookDto);
                book.BookAuthors = new List<BookAuthor>();
                foreach (var authorName in bookDto.Authors.Split(","))
                {
                    var author = _context.Authors.FirstOrDefault(x => x.Name == authorName);
                    if (author == null)
                    {
                        author = new Author { Name = authorName };
                        _context.Authors.Add(author);
                        _context.SaveChanges();
                    }
                    book.BookAuthors.Add(new BookAuthor
                    {
                        AuthorId = author.Id
                    });
                }
                _context.Books.Add(book);
                _context.SaveChanges();
            }
            

            return Ok(result);
        }
        public static object locker = new object();
        [HttpPost("[action]")]
        public IActionResult PreviewBooks([FromForm] IFormFile input)
        {
            string pathToFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (!System.IO.Directory.Exists(Path.Combine(pathToFolder, "TemporaryStorage")))
            {
                Directory.CreateDirectory(Path.Combine(pathToFolder, "TemporaryStorage"));
            }
            if (input == null)
            {
                return Ok();
            }
            string pathToFile = Path.Combine(pathToFolder, "TemporaryStorage", input.FileName);
            ExcelInputDto result;
            lock (locker)
            {
                System.IO.File.Delete(pathToFile);
                using (var stream = System.IO.File.Create(pathToFile))
                {
                    input.CopyTo(stream);
                }
                result = ExcelParcerUtil.ParseBook(pathToFile);
                System.IO.File.Delete(pathToFile);
            }


            return Ok(new { data = result.Books, recordsTotal = result.Books.Count(), recordsFiltered = result.Books.Count() });
        }

        [HttpGet("[action]")]
        public IActionResult GetCategories()
        {
            var result = _context.Books.Select(x => x.Category).Distinct().ToList();
            return Ok(result);
        }

        [HttpGet("[action]")]
        public IActionResult GetBookOnDate(PeriodType input)
        {
            switch (input)
            {
                case PeriodType.Day:
                    {
                        var result = _context.Books.GroupBy(x => x.PublishedOn).Select(x => new { Published = x.Key.ToString("dd.MM.yyyy"), Count = x.Count() });
                        return Ok(result);
                    }

                case PeriodType.Week:
                    {
                        var data = _context.Books.GroupBy(x => x.PublishedOn)
                            .Select(x => new { Published = x.Key, Count = x.Count() })
                            .ToList()
                            .OrderBy(x=>x.Published)
                            .ToList();
                        var result = new List<PeriodChartDto>();
                        var dateStart = new DateTime(data[0].Published.Year, data[0].Published.Month, 1);
                        var dateEnd = new DateTime(data[data.Count-1].Published.Year, data[data.Count - 1].Published.Month, 1);

                        for (; dateStart <= dateEnd; dateStart = dateStart.AddDays(7))
                        {
                            result.Add(new PeriodChartDto
                            {
                                Published = dateStart.ToString("dd.MM.yy") + "-" + dateStart.AddDays(6).ToString("dd.MM.yy"),
                                Count = data.Where(x => x.Published >= dateStart && x.Published <= dateStart.AddDays(6)).Sum(y => y.Count)
                            });
                        }

                        return Ok(result);
                    }

                case PeriodType.Month:
                    {
                        var result = _context.Books.GroupBy(x => x.PublishedOn)
                            .Select(x => new { Published = x.Key, Count = x.Count() })
                            .ToList()
                            .GroupBy(x => x.Published.ToString("MM.yy")).Select(x => new { Published = x.Key, Count = x.Sum(y=>y.Count) });
                        return Ok(result);
                    }
            }
            return Ok();
              
            
        }
    }
}
