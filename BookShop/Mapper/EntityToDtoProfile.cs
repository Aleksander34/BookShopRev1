using AutoMapper;
using BookShop.Core.Models;
using BookShop.Dto;

namespace BookShop.Mapper
{
    public class EntityToDtoProfile:Profile
    {
        public EntityToDtoProfile() 
        {
            this.CreateMap<Book, BookDto>()
            .ForMember(x => x.Authors, y => y.MapFrom(p => string.Join(",", p.BookAuthors.Select(t => t.Author.Name))))
             .ForMember(x => x.AvgStars, y => y.MapFrom(p => p.Reviews!=null&&p.Reviews.Any() ? Math.Round(p.Reviews.Average(a => a.NumStars),1):0));

            this.CreateMap<BookDto, Book>();
            this.CreateMap<Review, ReviewDto>()
                .ForMember(x => x.UserName, y => y.MapFrom(p => p.User.Name));
        }
    }
}
