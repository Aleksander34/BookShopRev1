using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.Core.Models
{
    public class Review : Entity
    {
        public int NumStars { get; set; }
        public string Comment { get; set; }
        public int UserId { get; set; }  //TODO:
        [ForeignKey("UserId")]
        public User User { get; set; }

        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public Book Book { get; set; }

        public override string ToString()
        {
            return $"{{ Имя: ID пользователя {User.Id}, Оценки: {NumStars}, Комментарии: {Comment}, Id Книги: {BookId}}}";
        }
    }
}
