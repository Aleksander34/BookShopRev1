namespace BookShop.Dto
{
    public class GetAllBookDto : PagedRequestDto
    {
        public string BookTitle { get; set; }
        public string AuthorName { get; set; }
        public string PublishedDateStart { get; set; }
        public string PublishedDateEnd { get; set; }
        public decimal? PriceStart { get; set; }
        public decimal? PriceEnd { get; set; }
        public string Category { get; set; }
    }
}
