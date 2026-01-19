using EateryCafeSimulator201.Models.Common;

namespace EateryCafeSimulator201.ViewModels.CheffViewModel
{
    public class CheffUpdateVM : BaseEntity
    {
        public IFormFile? Image { get; set; }
        public string Fullname { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
    }
}
