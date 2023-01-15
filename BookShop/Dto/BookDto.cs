using AutoMapper;
using BookShop.Core.Models;

namespace BookShop.Dto
{
    public class BookDto : EntityDto
    {
        public string Authors { get; set; }
        public int AvgStars { get; set; } //
        public PropertyDto Property { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishedOn { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int PropertyId { get; set; }

    }
}
