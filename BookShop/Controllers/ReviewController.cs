using AutoMapper;
using BookShop.Core;
using BookShop.Core.Models;
using BookShop.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : BookShopController
    {
        private readonly BookShopContext _context;
        private readonly IMapper _mapper;
        public ReviewController(BookShopContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpPost("[action]")]
        public IActionResult GetAll(GetAllReviewsDto input)
        {
            var totalCount = _context.Reviews
                .Where(x=>x.BookId == input.BookId)
                .Count();

            var reviews = _context.Reviews
                .Include(x=>x.User)
                .Where(x=>x.BookId == input.BookId)
                .Skip(input.SkipCount)
                .Take(input.CountOnPage)
                .ToList();

            var result = _mapper.Map<IEnumerable<ReviewDto>>(reviews);

            return Ok(new { data = result, recordsTotal = totalCount, recordsFiltered = totalCount }); 
        }

        [HttpPost("[action]")]
        public IActionResult Create(ReviewDto input)
        {
            var review = _mapper.Map<Review>(input);
            _context.Reviews.Add(review);
            _context.SaveChanges();

            return Ok($"Просмотр {review.Id} добавлен");
        }
        [HttpPost("[action]")]
        public IActionResult Remove(int id)
        {
            var review = _context.Reviews.Find(id);
            _context.Reviews.Remove(review);
            _context.SaveChanges();
            return Ok($"Просмотр {review.Id} удален ");
        }
        [HttpPost("[action]")]
        public IActionResult Update(ReviewDto input)
        {
            var review = _mapper.Map<Review>(input);
            _context.Reviews.Update(review);
            _context.SaveChanges();
            return Ok($"Просмотр {review.Id} обновлен");
        }
    }
}
