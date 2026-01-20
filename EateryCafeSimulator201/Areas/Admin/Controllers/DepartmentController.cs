using EateryCafeSimulator201.Contexts;
using EateryCafeSimulator201.Models;
using EateryCafeSimulator201.ViewModels.DepartmentViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EateryCafeSimulator201.Areas.Admin.Controllers;
[Area("Admin")]
public class DepartmentController(AppDbContext _context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var department = await _context.Departments.Select(opt => new DepGetVM()
        {
            Id = opt.Id,
            Name = opt.Name
        }).ToListAsync();
        return View(department);
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(DepCreateVM vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }
        Department dep = new()
        {
            Name = vm.Name
        };
        await _context.AddAsync(dep);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));

    }
    public async Task<IActionResult> Update(int id)
    {
        var dep = await _context.Departments.FindAsync(id);
        if(dep == null)
        {
            return NotFound();
        }
        DepUpdateVM vm = new()
        {
            Name = dep.Name
        };
        return View(vm);
    }
    [HttpPost]
    public async Task<IActionResult> Update(DepUpdateVM vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }
        var existDep = await _context.Departments.FindAsync(vm.Id);
        if(existDep == null)
        {
            ModelState.AddModelError("", "This department is not valid");
            return View(vm);
        }
        existDep.Name = vm.Name;
        _context.Departments.Update(existDep);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));


    }
    public async Task<IActionResult> Delete(int id)
    {
        var depDelete = await _context.Departments.FindAsync(id);
        if(depDelete == null)
        {
            return NotFound();
        }
        _context.Departments.Remove(depDelete);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    
}
