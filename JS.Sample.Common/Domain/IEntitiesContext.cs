using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JS.Sample.Common.Domain
{
    public interface IEntitiesContext : IDisposable
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : Entity;
        void SetAsAdded<TEntity>(TEntity entity) where TEntity : Entity;
        void SetAsModified<TEntity>(TEntity entity) where TEntity : Entity;
        void SetAsDeleted<TEntity>(TEntity entity) where TEntity : Entity;
        int SaveChanges();
        Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
