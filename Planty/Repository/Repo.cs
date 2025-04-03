using Blog_Platform.Data;
using Blog_Platform.IRepository;
using Blog_Platform.Models;
using Microsoft.EntityFrameworkCore;
using Planty.Data;

namespace Blog_Platform.Repository
{
    public class Repo<T> : IRepo<T>
        where T : class, IModelHelper
    {
        protected readonly ApplicationDbContext context;

        public Repo(ApplicationDbContext context)
        {
            this.context = context;
        }
        public void Add(T model)
        {
            context.Set<T>().Add(model);
        }

        public bool CheckIdExist(int Id)
        {
            return context.Set<T>().Any(x=>x.Id == Id);
        }
        public void Delete(int Id)
        {
            context.Set<T>().Where(x=>x.Id == Id).ExecuteDelete();
        }

        public List<T> GetAll()
        {
            return context.Set<T>().ToList();
        }

        public T? GetById(int Id)
        {
            return context.Set<T>().SingleOrDefault(x=>x.Id == Id);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(T model)
        {
            context.Update(model);
        }
    }
}
