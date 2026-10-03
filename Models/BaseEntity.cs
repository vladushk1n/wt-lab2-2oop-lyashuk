using System;

namespace wt_lab2_2oop.Models
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        protected BaseEntity(int id)
        {
            Id = id;
        }

        protected BaseEntity() { }

        // protected-член: служебный метод, доступный только внутри иерархии классов
        protected string GetBaseHeader()
        {
            return $"[ID: {Id} | Создан: {CreatedAt:yyyy-MM-dd}]";
        }

        public abstract void PrintInfo();
    }
}