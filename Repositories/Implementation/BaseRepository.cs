using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Eden_Fn.Models.Domain;
using Eden_Fn.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Eden_Fn.Repositories.Implementation
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly DatabaseContext ctx;
        public BaseRepository(DatabaseContext ctx)
        {
            this.ctx = ctx;
        }
        public bool Create(T entity)
        {
            try
            {
                ctx.Set<T>().Add(entity);
                ctx.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public bool Delete(T entity)
        {
            try
            {
                ctx.Set<T>().Remove(entity);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool Update(T entity)
        {
            try
            {
                ctx.Set<T>().Update(entity);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public T? FindById(int id)
        {
           return ctx.Set<T>().Find(id);
        }

        public IQueryable<T> GetAll()
        {
            return ctx.Set<T>().AsNoTracking();
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> expression)
        {
            return ctx.Set<T>().Where(expression).AsNoTracking();

        }

       
    }

}