using Blog_Platform.Models;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Blog_Platform.IRepository
{
    public interface IRepo<T>
        where T : class,IModelHelper
    {
        void Add(T model);
        void Update(T model);
        void Delete(int Id);
        List<T> GetAll();
        T? GetById(int Id);
        void Save();
        bool CheckIdExist(int Id);
    }
}
