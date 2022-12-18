namespace BookShop.Dto
{
    public class GetAllBookDto : PagedRequestDto
    {
        public string BookTitle { get; set; }
    }
}
