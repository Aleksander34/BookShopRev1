namespace BookShop.Core.Models
{
    public class User:Entity
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public TypeRole Role { get; set; }
        public override string ToString()
        {
            return $"{{ Имя: {Name}, Пароль: {Password}, Роль: {Role}}}";
        }
    }
}
