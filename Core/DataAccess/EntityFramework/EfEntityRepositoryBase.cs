using Core.DataAcces;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext, new() 
    {
        public void Add(TEntity entity)
        {
            //c# özel kod = using bitince otomatik çöpe atar. || IDısposable pattern implementation of c# altraftaki using isim metodu
            using (TContext context = new TContext())
            {
                var addedEntity = context.Entry(entity);
                addedEntity.State = EntityState.Added;
                context.SaveChanges();
            }
        }

        public void Delete(TEntity entity)
        {
            using (TContext context = new TContext())
            {

                var deletedEntity = context.Entry(entity);
                deletedEntity.State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            using (TContext context = new TContext())
            {
                // filtreli arama için
                return context.Set<TEntity>().SingleOrDefault(filter);
            }
        }

        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            using (TContext context = new TContext())
            {
                // talabe göre filtreli ya da hepsini araması için.
                return filter == null ? context.Set<TEntity>().ToList() : context.Set<TEntity>().Where(filter).ToList();
            }
        }

        public void Update(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                //var UpdatedEntity = context.Products.SingleOrDefault(p => p.ProductId == entity.ProductId);
                //UpdatedEntity.ProductName = entity.ProductName;
                //UpdatedEntity.CategoryId = entity.CategoryId;
                //UpdatedEntity.UnitPrice = entity.UnitPrice;
                //UpdatedEntity.UnitsInStock = entity.UnitsInStock;
                //UpdatedEntity.UnitsInStock = entity.UnitsInStock;
                //context.SaveChanges();

                // Yukarıdaki kod bir linq örneği iken ve tek tek güncellerken alt taraftaki ise EntityFramework yöntemidir ve oda uygun şekilde göncelleme yapar.
                var updatedEntity = context.Entry(entity);
                updatedEntity.State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
