using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace JoseHerrera_WebApi.Models
{
    public class DepartmentDTO
    {
        public int ID { get; set; }

        [Display(Name = "Department Name")]
        [Required(ErrorMessage = "The Department Name cannot be left blank.")]
        [StringLength(50, ErrorMessage = "Department name cannot be more than 50 characters long.")]
        public string DepartmentName { get; set; }

        public ICollection<EmployeeDTO> Employees { get; set; } = new HashSet<EmployeeDTO>();
    }
}
