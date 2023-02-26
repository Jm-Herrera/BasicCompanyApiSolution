using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace JoseHerrera_WebApi.Models
{
    public class Employee
    {
        public int ID { get; set; }

        [Display(Name = "Employee")]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public string Position
        {
            get
            {
                return JobTitle + ", " + Department?.DepartmentName + " Department.";
            }
        }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "The first name cannot be left blank.")]
        [StringLength(50, ErrorMessage = "First name cannot be more than 50 characters long.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "The last name cannot be left blank.")]
        [StringLength(100, ErrorMessage = "Last name cannot be more than 100 characters long.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "You must enter the 9 digit SIN")]
        [StringLength(9)]//DS Note: we only include this to limit the size of the database field to 10
        [RegularExpression("^\\d{9}$", ErrorMessage = "The SIN number must be exactly 9 numeric digits.")]
        public string SIN { get; set; }

        [Display(Name = "Start Date")]
        [Required(ErrorMessage = "You must enter the start date.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "You must enter the salary amount.")]
        [Range(0.0d, 9999999.0d, ErrorMessage = "Invalid salary amount.")]
        public Double Salary { get; set; }

        [Display(Name = "Job Title")]
        [Required(ErrorMessage = "The job title is required.")]
        [StringLength(50, ErrorMessage = "Job title cannot be more than 50 characters long.")]
        public string JobTitle { get; set; }

        [Display(Name = "Department")]
        [Required(ErrorMessage = "You must select the Department.")]
        public int DepartmentID { get; set; }
        public Department Department { get; set; }
    }
}
