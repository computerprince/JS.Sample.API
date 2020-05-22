using JS.Sample.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JS.Sample.Percistance.EntityConfigurations
{
    internal class ClientRequestConfiguration
           : IEntityTypeConfiguration<ClientRequest>
    {
        public void Configure(EntityTypeBuilder<ClientRequest> builder)
        {
            builder.ToTable("Requests");
            builder.HasKey(cr => cr.Id);
            builder.Property(cr => cr.Name).IsRequired();
            builder.Property(cr => cr.ContentId).IsRequired();
            builder.Property(cr => cr.Content).IsRequired();
            builder.Property(cr => cr.Time).IsRequired();

        }
    }
}
