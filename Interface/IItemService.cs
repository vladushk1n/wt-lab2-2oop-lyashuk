using System.Collections.Generic;
using System.Threading.Tasks;

namespace wt_lab2_2oop.Interfaces
{
    public interface IItemService<T>
    {
        void Add(T item);
        List<T> GetAll();
        T GetById(int id);
        bool Delete(int id);

        // --- БЛОК 3: Асинхронный метод с возвращаемым значением Task<List<T>> ---
        Task<List<T>> GetCollectionFromDbAsync();
    }
}