using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.Core.Models
{
    public class BookAuthor
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public Book Book { get; set; }

        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public Author Author { get; set; }
    }
}
