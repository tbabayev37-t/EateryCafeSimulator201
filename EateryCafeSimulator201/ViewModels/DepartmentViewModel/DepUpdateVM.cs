using EateryCafeSimulator201.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace EateryCafeSimulator201.ViewModels.DepartmentViewModel
{
    public class DepUpdateVM:BaseEntity
    {
        [Required]
        public string Name { get; set; }=string.Empty;
    }
}
