using EateryCafeSimulator201.Models.Common;

namespace EateryCafeSimulator201.ViewModels.CheffViewModel
{
    public class CheffGetVM:BaseEntity
    {
        public string ImagePath { get; set; }= string.Empty;
        public string Fullname { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
    }
}
