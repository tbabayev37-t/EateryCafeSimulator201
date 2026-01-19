using System.ComponentModel.DataAnnotations.Schema;
using EateryCafeSimulator201.Models.Common;

namespace EateryCafeSimulator201.Models
{
    public class Cheff:BaseEntity
    {
        public string ImagePath { get;set; }=string.Empty;
        public string Fullname { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public Department Department { get; set; } = null!;
    }
}
