using System;

namespace wt_lab2_2oop.Models
{
    public class Player : BaseEntity
    {
        public string Nickname { get; set; }
        public string Role { get; set; }
        public int Rating { get; private set; }

        public Player(int id, string nickname, string role, int rating) : base(id)
        {
            Nickname = nickname;
            Role = role;
            UpdateRating(rating);
        }

        public Player() { Nickname = ""; Role = ""; }

        public void UpdateRating(int newRating)
        {
            if (newRating < 0)
                throw new ArgumentException("Рейтинг не может быть отрицательным!");
            Rating = newRating;
        }

        // Вызов protected-метода GetBaseHeader() из базового класса BaseEntity
        public override void PrintInfo()
        {
            Console.WriteLine($"{GetBaseHeader()} Игрок: {Nickname,-10} | Роль: {Role,-8} | Рейтинг: {Rating} MMR");
        }
    }
}