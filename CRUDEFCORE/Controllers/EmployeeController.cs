using CRUDEFCORE.Models;
using CRUDEFCORE.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUDEFCORE.Controllers;

public class EmployeeController : Controller
{
    private readonly DatabaseContext _databaseContext;

    public EmployeeController( DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }



    public async Task<IActionResult> Index()
    {
        var employes = await _databaseContext.Employees.ToListAsync();
        return View(employes);

    }


    // GET
    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
    {
        var employe = new Employee()
        {
            Name = addEmployeeRequest.Name,
            Email = addEmployeeRequest.Email,
            DateOfBirth = addEmployeeRequest.DateOfBirth,
            Salary = addEmployeeRequest.Salary,
            Department = addEmployeeRequest.Department
        };
        await _databaseContext.Employees.AddAsync(employe);
        await _databaseContext.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpGet]

    public async Task<IActionResult> View(int id)
    {
        var employe = await _databaseContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
        if (employe!=null)
        {
            var viewmodel = new UpdateEmployeeModel()
            {
                Id = employe.Id,
                Name = employe.Name,
                Email = employe.Email,
                DateOfBirth = employe.DateOfBirth,
                Salary = employe.Salary,
                Department = employe.Department
            };
            return await Task.Run(() => View("View", viewmodel));
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> View(UpdateEmployeeModel model)
    {
        var employee = await _databaseContext.Employees.FindAsync(model.Id);
        if (employee!=null)
        {
            employee.Name = model.Name;
            employee.Email = model.Email;
            employee.Salary = model.Salary;
            employee.DateOfBirth = model.DateOfBirth;
            employee.Department = model.Department;
            await _databaseContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(UpdateEmployeeModel model)
    {
        var employe = await _databaseContext.Employees.FindAsync(model.Id);
        if (employe!=null)
        {
            _databaseContext.Employees.Remove(employe);
            await _databaseContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        return RedirectToAction("Index");
    }
}