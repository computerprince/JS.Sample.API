using JS.Sample.Common.Domain;
using JS.Sample.Domain;
using JS.Sample.Percistance.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;


namespace JS.Sample.Percistance
{
  

    public class SampleDbContext : DbContext, IEntitiesContext
    {
        public SampleDbContext()
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ClientRequest> ClientRequest { get; set; }


        private static readonly object Lock = new object();


        private static bool _databaseInitialized;

        public SampleDbContext(DbContextOptions<SampleDbContext> options) : base(options)
        {
            if (_databaseInitialized)
            {
                return;
            }
            lock (Lock)
            {
                if (!_databaseInitialized)
                {
                    // Set the database intializer which is run once during application start
                    _databaseInitialized = true;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
     
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new ClientRequestConfiguration());

        }

        public new DbSet<TEntity> Set<TEntity>() where TEntity : Entity
        {
            return base.Set<TEntity>();
        }

        public void SetAsAdded<TEntity>(TEntity entity) where TEntity : Entity
        {
            UpdateEntityState(entity, EntityState.Added);
        }

        public void SetAsModified<TEntity>(TEntity entity) where TEntity : Entity
        {
            UpdateEntityState(entity, EntityState.Modified);
        }

        public void SetAsDeleted<TEntity>(TEntity entity) where TEntity : Entity
        {
            UpdateEntityState(entity, EntityState.Deleted);
        }

        private void UpdateEntityState<TEntity>(TEntity entity, EntityState entityState) where TEntity : Entity
        {
            var dbEntityEntry = GetDbEntityEntrySafely(entity);
            dbEntityEntry.State = entityState;
        }

        private EntityEntry GetDbEntityEntrySafely<TEntity>(TEntity entity) where TEntity : Entity
        {
            var dbEntityEntry = Entry<TEntity>(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                Set<TEntity>().Attach(entity);
            }
            return dbEntityEntry;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await base.SaveChangesAsync();
            return true;
        }

    }

    public class DataContextDesignFactory : IDesignTimeDbContextFactory<SampleDbContext>
    {
        public IConfiguration _configuration { get; }

        public DataContextDesignFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DataContextDesignFactory()
        {
        }

        public SampleDbContext CreateDbContext(string[] args)
        {
            var connectionString =

                     @"Data Source=./;Initial Catalog=ShopDatabase;Trusted_Connection=False;MultipleActiveResultSets=true;";

            var optionsBuilder = new DbContextOptionsBuilder<SampleDbContext>()
                .UseSqlServer(connectionString);

            return new SampleDbContext(optionsBuilder.Options);
        }
    }
}
