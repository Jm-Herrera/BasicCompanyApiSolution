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
    public class DepartmentsController : ControllerBase
    {
        private readonly JHContext _context;

        public DepartmentsController(JHContext context)
        {
            _context = context;
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentDTO>>> GetDepartments()
        {
            return await _context.Departments
                .Select(d => new DepartmentDTO
                {
                    ID = d.ID,
                    DepartmentName = d.DepartmentName
                   
                })
                .ToListAsync();
        }

        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentDTO>> GetDepartment(int id)
        {
            var departmentDTO = await _context.Departments
                .Select(d=> new DepartmentDTO
                {
                    ID = d.ID,
                    DepartmentName = d.DepartmentName
                })
                .FirstOrDefaultAsync(p => p.ID == id);

            if (departmentDTO == null)
            {
                return NotFound(new {message = "Error: Department not found."});
            }

            return departmentDTO;
        }

        // PUT: api/Departments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartment(int id, DepartmentDTO department)
        {
            if (id != department.ID)
            {
                return BadRequest(new { message = "Error: Incorrect ID for Department." });
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var depToUpdate = await _context.Departments.FindAsync(id);

            //Check that you got it
            if (depToUpdate == null)
            {
                return NotFound(new { message = "Error: Department record not found." });
            }

            depToUpdate.ID = department.ID;
            depToUpdate.DepartmentName = department.DepartmentName;


            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator." });
            }



        }

        // POST: api/Departments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Department>> PostDepartment(DepartmentDTO department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Department depto = new Department
            {
                DepartmentName = department.DepartmentName
            };
            try
            {
                _context.Departments.Add(depto);
                await _context.SaveChangesAsync();
                department.ID = depto.ID;
                return CreatedAtAction(nameof(GetDepartment), new { id = department.ID }, department);

            }
            catch (DbUpdateException)
            {
                return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator." });
            }


        }

        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound(new { message = "Delete Error: Department has already been removed." });
            }

            try
            {
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();

                return NoContent();

            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("FOREIGN KEY constraint failed"))
                {
                    return BadRequest(new { message = "Delete Error: Remember, you cannot delete a Department that has employees assigned." });
                }
                else
                {
                    return BadRequest(new { message = "Delete Error: Unable to delete Department. Try again, and if the problem persists see your system administrator." });
                }
            }

        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.ID == id);
        }
    }
}
