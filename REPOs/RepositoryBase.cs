using BOs.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace REPOs
{
    public class RepositoryBase<T> where T : class 
    {
        private readonly OldCarShowroomNetworkContext _context;
        private readonly DbSet<T> _dbSet;
        public RepositoryBase()
        {
            _context = new OldCarShowroomNetworkContext();
            _dbSet = _context.Set<T>();
        }
        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }
        public void Add(T item)
        {
            _dbSet.Add(item);
            _context.SaveChanges();

        }
        public void Delete(T item)
        {
            _dbSet.Remove(item);
            _context.SaveChanges();
        }
        public void Update(T item)
        {
            var tracker = _context.Attach(item);
            tracker.State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}