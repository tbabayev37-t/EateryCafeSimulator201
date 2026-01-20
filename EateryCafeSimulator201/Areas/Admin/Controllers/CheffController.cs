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
        var cheff = await _context.Cheffs.Select(opt => new CheffGetVM()
        {
            Id = opt.Id,
            Fullname = opt.Fullname,
            Description=opt.Description,
            DepartmentName=opt.Department.Name
        }).ToListAsync();
        return View(cheff);
    }
    public async Task<IActionResult> Create()
    {
        await SendDepartmentWithViewBag();
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(CheffCreateVM vm)
    {
        await SendDepartmentWithViewBag();
        if (!ModelState.IsValid)
        {
            return View(vm);
        }
        var existDepartment = await _context.Departments.AnyAsync(x => x.Id == vm.DepartmentId);
        if (!existDepartment)
        {
            ModelState.AddModelError("", "This department is nt valid");
            return View(vm);
        }
        if (vm.Image.Length > 2 * 1024 * 1024)
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
            Fullname= cheff.Fullname,
            Description = cheff.Description,
            DepartmentId = cheff.DepartmentId
        };
        await SendDepartmentWithViewBag();
        return View(vm);
    }
    [HttpPost]
    public async Task<IActionResult> Update(CheffUpdateVM vm)
    {
        await SendDepartmentWithViewBag();
        if (!ModelState.IsValid)
        {
            return View(vm);
        }
        var existcheff = await _context.Cheffs.FindAsync(vm.Id);
        if(existcheff == null)
        {
            return NotFound();
        }
        var department = await _context.Departments.AnyAsync(x => x.Id == vm.DepartmentId);
        if(department is false)
        {
            ModelState.AddModelError("DepartmentId", "This department is not valid");
            return View(vm);
        }
        if (!vm.Image?.CheckSize(2) ?? false)
        {
            ModelState.AddModelError("Image", "Image must be maximum 2 mb");
            return View(vm);
        }
        if (!vm.Image?.CheckType("image") ?? false)
        {
            ModelState.AddModelError("Image", "File must be only image format");
            return View(vm);
        }
        existcheff.Fullname=vm.Fullname;
        existcheff.Description=vm.Description;
        existcheff.DepartmentId=vm.DepartmentId;
        if(vm.Image is { })
        {
            string deletedPath = Path.Combine(FolderPath, existcheff.ImagePath);
            FileHelper.DeleteFile(deletedPath);
            string newImagePath = await vm.Image.UploadFile(FolderPath);
            existcheff.ImagePath = newImagePath;
        }
        _context.Cheffs.Update(existcheff);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult>Delete(int id)
    {
        var cheff = await _context.Cheffs.FindAsync(id);
        if(cheff == null)
        {
            return NotFound();
        }
        _context.Cheffs.Remove(cheff);
        await _context.SaveChangesAsync();
        string deletedPath = Path.Combine(FolderPath, cheff.ImagePath);
        FileHelper.DeleteFile(deletedPath);
        return RedirectToAction(nameof(Index));
    }
    public async Task SendDepartmentWithViewBag()
    {
        var department=  await _context.Departments.Select(t => new SelectListItem() { Text = t.Name, Value = t.Id.ToString() }).ToListAsync();
        ViewBag.Departments = department;
    }
}
