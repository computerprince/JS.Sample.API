using JS.Sample.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JS.Sample.Percistance.EntityConfigurations
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");
            builder.HasKey(o => o.Id);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(60);
            builder.Property(p => p.Price).IsRequired();
            builder.Property(p => p.ManufactureDate).IsRequired();
            builder.Property(p => p.Location).IsRequired();
            builder.Property(p => p.IsAvailable).IsRequired();



        }
    }
}
