using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JoseHerrera_WebApi.Data;
using JoseHerrera_WebApi.Models;

namespace JoseHerrera_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly JHContext _context;

        public EmployeesController(JHContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployees()
        {
            return await _context.Employees
                .Include(d => d.Department)
                .Select(e => new EmployeeDTO
                {
                    ID = e.ID,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    SIN = e.SIN,
                    StartDate = e.StartDate,
                    Salary = e.Salary,
                    JobTitle = e.JobTitle,
                    DepartmentID = e.DepartmentID,
                    Department = new DepartmentDTO
                    {
                        ID = e.Department.ID,
                        DepartmentName = e.Department.DepartmentName
                    }
                })
                .ToListAsync();
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployee(int id)
        {
            var employee = await _context.Employees
                .Include(_ => _.Department)
                .Select(e => new EmployeeDTO
                {
                    ID = e.ID,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    SIN = e.SIN,
                    StartDate = e.StartDate,
                    Salary = e.Salary,
                    JobTitle = e.JobTitle,
                    DepartmentID = e.DepartmentID,
                    Department = new DepartmentDTO
                    {
                        ID = e.Department.ID,
                        DepartmentName = e.Department.DepartmentName
                    }
                })
                .FirstOrDefaultAsync(p => p.ID == id);

            if (employee == null)
            {
                return NotFound(new { message = "Error: Employee record not found." });
            }

            return employee;
        }

        // GET: api/EmployeesByDepartment
        [HttpGet("ByDepartment/{id}")]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployeesByDepartment(int id)
        {
            var employeeDTOs = await _context.Employees
                .Include(_ => _.Department)
                .Select(d => new EmployeeDTO
                {
                    ID = d.ID,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    SIN = d.SIN,
                    StartDate = d.StartDate,
                    Salary = d.Salary,
                    JobTitle = d.JobTitle,
                    DepartmentID = d.DepartmentID,
                    Department = new DepartmentDTO
                    {
                        ID = d.Department.ID,
                        DepartmentName = d.Department.DepartmentName
                    }
                })
                .Where(e => e.DepartmentID == id)
                .ToListAsync();

            if (employeeDTOs.Count() > 0)
            {
                return employeeDTOs;
            }
            else
            {
                return NotFound(new { message = "Error: No Employee records for that Department." });
            }
        }


        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, EmployeeDTO employee)
        {
            if (id != employee.ID)
            {
                return BadRequest(new { message = "Error: ID does not match Employee" });
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Get the record you want to update
            var employeeToUpdate = await _context.Employees.FindAsync(id);

            //Check that you got it
            if (employeeToUpdate == null)
            {
                return NotFound(new { message = "Error: Employee record not found." });
            }

            //Update the properties
            employeeToUpdate.ID = employee.ID;
            employeeToUpdate.FirstName = employee.FirstName;
            employeeToUpdate.LastName = employee.LastName;
            employeeToUpdate.SIN = employee.SIN;
            employeeToUpdate.StartDate = employee.StartDate;
            employeeToUpdate.Salary = employee.Salary;
            employeeToUpdate.JobTitle = employee.JobTitle;
            employeeToUpdate.DepartmentID = employee.DepartmentID;

            //_context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("UNIQUE"))
                {
                    return BadRequest(new { message = "Unable to save: Duplicate SIN number." });
                }
                else
                {
                    return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator." });
                }



            }
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmployeeDTO>> PostEmployee(EmployeeDTO employee)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Employee emp = new Employee
            {
                ID = employee.ID,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                SIN = employee.SIN,
                StartDate = employee.StartDate,
                Salary = employee.Salary,
                JobTitle = employee.JobTitle,
                DepartmentID = employee.DepartmentID
            };
            try
            {
                _context.Employees.Add(emp);
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("UNIQUE"))
                {
                    return BadRequest(new { message = "Unable to save: Duplicate SIN number." });
                }
                else
                {
                    return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator." });
                }

            }


            return CreatedAtAction("GetEmployee", new { id = employee.ID }, employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound(new { message = "Delete Error: Employee has already been removed." });
            }
            try
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();

                return NoContent();

            }
            catch (DbUpdateException)
            {
                return BadRequest(new { message = "Delete Error: Unable to delete Employee." });
            }
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.ID == id);
        }
    }
}
