using AutoMapper;
using BookShop.Core.Models;

namespace BookShop.Dto
{
    [AutoMap(typeof(Book), ReverseMap = true)]
    public class BookDto : EntityDto
    {
        public ICollection<AuthorDto> Authors { get; set; } // Это не выводится но может быть в запросе использовано при связывании
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishedOn { get; set; }
        public string Publisher { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int PropertyId { get; set; }

    }
}
