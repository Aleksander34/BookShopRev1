namespace BookShop.Dto
{
    public class GetAllReviewsDto: PagedRequestDto
    {
        public int BookId { get; set; }
    }
}
