using System.ComponentModel.DataAnnotations;

namespace EateryCafeSimulator201.ViewModels.DepartmentViewModel
{
    public class DepCreateVM
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
