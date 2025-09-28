using Orders.Data;
using Microsoft.EntityFrameworkCore;

namespace Orders.DataAccess
{
    public class Repository<T> where T : class
    {
        private readonly MyAppContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(MyAppContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public T? GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
            //var orders = _dbSet.FromSqlRaw("Exec hello").ToList();
            //return orders;
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
        public IQueryable<T> Query()
        {
            return _dbSet.AsQueryable();
        }

    }
}
