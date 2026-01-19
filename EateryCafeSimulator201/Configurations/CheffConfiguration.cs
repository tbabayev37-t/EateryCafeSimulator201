using EateryCafeSimulator201.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EateryCafeSimulator201.Configurations
{
    public class CheffConfiguration : IEntityTypeConfiguration<Cheff>
    {
        public void Configure(EntityTypeBuilder<Cheff> builder)
        {
            builder.Property(x => x.Fullname).IsRequired().HasMaxLength(64);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(1024);

            builder.HasOne(x => x.Department).WithMany(x => x.Cheffs).HasForeignKey(x => x.DepartmentId).HasPrincipalKey(x => x.Id).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
