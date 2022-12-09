namespace BookShop.Core.Models
{
    public class Property : Entity
    {
        public string Color { get; set; }
        public string BindingType { get; set; }
        public string Condition { get; set; }

        public ICollection<Book> Books { get; set; }

        public override string ToString()
        {
            return $"{{ Свойства: {Id}, Цвет: {Color}, Переплет: {BindingType}, Состояние: {Condition}}}";
        }
    }
}
