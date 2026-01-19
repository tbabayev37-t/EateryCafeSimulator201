namespace EateryCafeSimulator201.ViewModels.CheffViewModel
{
    public class CheffCreateVM
    {
        public IFormFile Image { get; set; } = null!;
        public string Fullname { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
    }
}
