using EateryCafeSimulator201.Contexts;
using EateryCafeSimulator201.Helpers;
using EateryCafeSimulator201.Models;
using EateryCafeSimulator201.ViewModels.CheffViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace EateryCafeSimulator201.Areas.Admin.Controllers;
[Area("Admin")]
public class CheffController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly string FolderPath;
    public CheffController(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
        FolderPath = Path.Combine(_environment.WebRootPath, "images");      
    }
    public async Task<IActionResult> Index()
    {
        var cheffs = await _context.Cheffs.Select(opt => new CheffGetVM()
        {
            Id = opt.Id,
            ImagePath = opt.ImagePath,
            Fullname = opt.Fullname,
            Description = opt.Description,
            DepartmentName = opt.Department.Name
        }).ToListAsync();
        return View(cheffs);
    }
    public async Task<IActionResult> Create()
    {
        await SendDepartmentWithViewModel();
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(CheffCreateVM vm)
    {
        await SendDepartmentWithViewModel();
        if (!ModelState.IsValid)
        {
            return View(vm);
        }
        var existDepartment = await _context.Departments.AnyAsync(x=>x.Id == vm.DepartmentId);
        if (!existDepartment)
        {
            ModelState.AddModelError("", "This department is not valid");
            return View(vm);
        }
        if(vm.Image.Length > 2 * 1024 * 1024)
        {
            ModelState.AddModelError("image", "Image maximum size must be 2 mb");
            return View(vm);
        }
        if (!vm.Image.ContentType.Contains("image"))
        {
            ModelState.AddModelError("image", "File must be only image format");
            return View(vm);
        }
        string uniqueFileNmae = await vm.Image.UploadFile(FolderPath);
        Cheff cheff = new()
        {
            Fullname = vm.Fullname,
            Description = vm.Description,
            DepartmentId = vm.DepartmentId,
            ImagePath = uniqueFileNmae            
        };
        await _context.AddAsync(cheff);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Update(int id)
    {
        var cheff = await _context.Cheffs.FindAsync(id);
        if(cheff == null)
        {
            return NotFound();
        }
        CheffUpdateVM vm = new()
        {
            Id = cheff.Id,
            Fullname = cheff.Fullname,
            Description = cheff.Description,
            DepartmentId = cheff.DepartmentId
        };
        await SendDepartmentWithViewModel();
        return View(vm);
    }
    [HttpPost]
    public async Task<IActionResult> Update(CheffUpdateVM vm)
    {
        await SendDepartmentWithViewModel();
        if (!ModelState.IsValid)
        {
            return View(vm);
        }
        var cheff = await _context.Cheffs.FindAsync(vm.Id);
        if(cheff is null)
        {
            return NotFound();
        }
        var isExistDepartment = await _context.Departments.AnyAsync(x => x.Id == vm.DepartmentId);
        if(!isExistDepartment)
        {
            ModelState.AddModelError("DepartmentId", "This department is not valid");
            return View(vm);
        }
        if (!vm.Image?.CheckSize(2) ?? false)
        {
            ModelState.AddModelError("Image", "This department is not valid");
            return View(vm);
        }
        if (!vm.Image?.CheckType("image") ?? false)
        {
            ModelState.AddModelError("Image", "Only image format please!!!");
            return View(vm);
        }
        cheff.Fullname = vm.Fullname;
        cheff.Description = vm.Description;
        cheff.DepartmentId=vm.DepartmentId;
        if(vm.Image is not null)
        {
            string deletedPath = Path.Combine(FolderPath, cheff.ImagePath);
            FileHelper.DeleteFile(deletedPath);
            string newImagePath = await vm.Image.UploadFile(FolderPath);
            cheff.ImagePath = newImagePath;
        }
        _context.Cheffs.Update(cheff);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Delete(int id)
    {
        var deletedCheff = await _context.Cheffs.FindAsync(id);
        if(deletedCheff is null)
        {
            return NotFound();
        }
        _context.Cheffs.Remove(deletedCheff);
        await _context.SaveChangesAsync();
        string deletedPath = Path.Combine(FolderPath, deletedCheff.ImagePath);
        FileHelper.DeleteFile(deletedPath);
        return RedirectToAction(nameof(Index));
    }
    
    public async Task SendDepartmentWithViewModel()
    {
        var department = await _context.Departments.Select(t => new SelectListItem() { Text = t.Name, Value = t.Id.ToString() }).ToListAsync();
        ViewBag.Departments = department;
    }
}
