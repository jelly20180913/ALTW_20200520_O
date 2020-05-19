using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
namespace WebApi.Models.Repository.EDI.Interface
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        string Create(TEntity instance);

        void Update(TEntity instance);

        void Delete(TEntity instance);

        TEntity Get(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> GetAll();

        void SaveChanges();

        List<string> CreateBatch(List<TEntity> instance);

        List<string> test(List<PosData> instance);
    }
}
