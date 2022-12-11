using AutoMapper;
using BookShop.Core;
using BookShop.Core.Models;
using BookShop.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : BookShopController
    {
        private readonly BookShopContext _context;
        private readonly IMapper _mapper;
        public PropertyController(BookShopContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet("[action]")]
        public IActionResult GetAll()
        {
            var properties = _context.Properties.ToList();
            var result = _mapper.Map<IEnumerable<PropertyDto>>(properties);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public IActionResult Create(PropertyDto input)
        {
            var property = _mapper.Map<Property>(input);
            _context.Properties.Add(property);
            _context.SaveChanges();

            return Ok($"Свойство {property.Id} добавлено");
        }
        [HttpPost("[action]")]
        public IActionResult Remove(int id)
        {
            var property = _context.Properties.Find(id);
            _context.Properties.Remove(property);
            _context.SaveChanges();
            return Ok($"Свойство {property.Id} удалено ");
        }
        [HttpPost("[action]")]
        public IActionResult Update(PropertyDto input)
        {
            var property = _mapper.Map<Property>(input);
            _context.Properties.Update(property);
            _context.SaveChanges();
            return Ok($"Свойства {property.Id} обновлено");
        }
    }
}
