namespace BookShop.Dto
{
    public class GetAllBookDto : PagedRequestDto
    {
        public string BookTitle { get; set; }
        public string AuthorName { get; set; }
        public DateTime? PublishedDateStart { get; set; }
        public DateTime? PublishedDateEnd { get; set; }
    }
}
