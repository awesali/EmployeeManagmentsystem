using System.Runtime.InteropServices;
using EmployeeManagmentsystem.Data;
using EmployeeManagmentsystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagmentsystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeDb _context;

        public EmployeeController(EmployeeDb context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Getall()
        { 
            return Ok(_context.employees.ToList());
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            _context.employees.Add(employee);
            _context.SaveChanges();
            return Created("", employee);
        }

        [HttpPut("{Id}")]
        public IActionResult Update(int Id, Employee UpdatedEmployee)
        {
            var employee = _context.employees.Find(Id);
            if (employee == null) return NotFound();

            employee.Name = UpdatedEmployee.Name;
            employee.Designation = UpdatedEmployee.Designation;
            employee.Salary = UpdatedEmployee.Salary;
            employee.Department = UpdatedEmployee.Department;


            _context.SaveChanges();
            return NotFound();
        }

        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            var employee= _context.employees.Find(Id);

            if (employee == null) return NotFound();
            _context.employees.Remove(employee);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
