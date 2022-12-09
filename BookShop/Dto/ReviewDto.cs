using AutoMapper;
using BookShop.Core.Models;

namespace BookShop.Dto
{
    [AutoMap(typeof(Review), ReverseMap = true)]
    public class ReviewDto:EntityDto
    {
        //public BookDto Book { get; set; } Это не выводится но может быть в запросе при связывании использовано
        public int BookId { get; set; }
        public string Name { get; set; }
        public int NumStars { get; set; }
        public string Comment { get; set; }
    }
}
