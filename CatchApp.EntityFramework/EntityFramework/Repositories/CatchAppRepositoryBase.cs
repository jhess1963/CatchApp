using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace CatchApp.EntityFramework.Repositories
{
    public abstract class CatchAppRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<CatchAppDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected CatchAppRepositoryBase(IDbContextProvider<CatchAppDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
            
        }

        //add common methods for all repositories
    }

    public abstract class CatchAppRepositoryBase<TEntity> : CatchAppRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected CatchAppRepositoryBase(IDbContextProvider<CatchAppDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
