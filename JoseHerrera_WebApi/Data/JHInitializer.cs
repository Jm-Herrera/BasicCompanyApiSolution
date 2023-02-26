using JoseHerrera_WebApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace JoseHerrera_WebApi.Data
{
    public static class JHInitializer
    {

        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            JHContext context = applicationBuilder.ApplicationServices.CreateScope()
                .ServiceProvider.GetRequiredService<JHContext>();
            try
            {
                //Create the database if it does not exist and apply the Migration
                context.Database.Migrate();

                if (context.Departments.Count() == 0)
                {
                    var departments = new List<Department>
                    {
                        new Department { DepartmentName="Accounting" },
                        new Department { DepartmentName="Information Systems" },
                        new Department { DepartmentName="Human Resources" },
                        new Department { DepartmentName="Marketing" }
                    };
                    departments.ForEach(d => context.Departments.Add(d));
                    context.SaveChanges();
                }
                int departmentCount = context.Departments.Count();

                if (context.Employees.Count() == 0)
                {
                    //No employees so we will ADD some.  
                    string[] firstNames = new string[] { "Fred", "Barney", "Wilma", "Betty", "Dave", "Tim" };
                    string[] lastsNames = new string[] { "Stovell", "Jones", "Bloggs", "Flintstone", "Rubble", "Brown" };
                    string[] jobNames = new string[] { "Programmer", "Clerk", "Designer", "Supervisor", "Executive Assistant", "Security Guard", "Gopher" };

                    //Create the employees with some random values
                    Random random = new Random(99);

                    // Startdate for randomly produced employees 
                    DateTime startDate = new DateTime(2022, 2, 22);

                    foreach (string lastName in lastsNames)
                    {
                        foreach (string firstname in firstNames)
                        {
                            //Construct some employee details
                            Employee newEmployee = new Employee();
                            newEmployee.FirstName = firstname;
                            newEmployee.LastName = lastName;
                            newEmployee.SIN = random.Next(213214131, 989898989).ToString();
                            newEmployee.JobTitle = jobNames[random.Next(jobNames.Count())];
                            newEmployee.Salary = Math.Round(Convert.ToDouble(random.Next(10000, 50000) * Math.PI), 2);
                            newEmployee.StartDate = startDate.AddDays(-random.Next(1500));
                            newEmployee.DepartmentID = random.Next(1, departmentCount + 1);
                            context.Employees.Add(newEmployee);
                            context.SaveChanges();
                        }
                    }
                }



            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.GetBaseException().Message);
            }







        }



    }
}
