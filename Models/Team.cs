using System;

namespace wt_lab2_2oop.Models
{
    // Наследуемся от базового класса BaseEntity
    public class Team : BaseEntity
    {
        public string Name { get; set; }
        public string Tag { get; set; }

        public Team(int id, string name, string tag) : base(id)
        {
            Name = name;
            Tag = tag;
        }

        // Переопределение абстрактного метода базового класса
        public override void PrintInfo()
        {
            Console.WriteLine($"[Команда #{Id}] {Name,-15} | Тэг: [{Tag}] | Добавлена: {CreatedAt:yyyy-MM-dd}");
        }
    }
}