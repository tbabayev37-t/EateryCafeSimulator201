using EateryCafeSimulator201.Models.Common;

namespace EateryCafeSimulator201.Models
{
    public class Department:BaseEntity
    {
        public string Name { get; set; } = null!;
        public ICollection<Cheff> Cheffs { get; set; } = [];

    }
}
