using AutoMapper;
using BookShop.Core;
using BookShop.Core.Models;
using BookShop.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : BookShopController
    {
        private readonly BookShopContext _context;
        private readonly IMapper _mapper;
        public AuthorController(BookShopContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet("[action]")]
        public IActionResult GetAll()
        {
            var authors = _context.Authors.ToList();
            var result = _mapper.Map<IEnumerable<AuthorDto>>(authors);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public IActionResult Create(AuthorDto input)
        {
            var author = _mapper.Map<Author>(input);
            _context.Authors.Add(author);
            _context.SaveChanges();

            return Ok($"Автор {author.Name} добавлен");
        }
        [HttpPost("[action]")]
        public IActionResult Remove(int id)
        {
            var author = _context.Authors.Find(id);
            _context.Authors.Remove(author);
            _context.SaveChanges();
            return Ok($"Автор {author.Name} удален ");
        }
        [HttpPost("[action]")]
        public IActionResult Update(AuthorDto input)
        {
            var author = _mapper.Map<Author>(input);
            _context.Authors.Update(author);
            _context.SaveChanges();
            return Ok($"Автор {author.Id} обновлен");
        }
    }
}
