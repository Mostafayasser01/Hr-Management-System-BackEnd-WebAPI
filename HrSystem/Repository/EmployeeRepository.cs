using HrSystem.DTO;
using HrSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HrSystem.Repository
{
    public class EmployeeRepository:IEmployeeRepository
    {
        ITIContext db;
        public EmployeeRepository(ITIContext context)
        {
            db = context;   
        }
        public Employee Add(AddEmployeeDTO EmployeeDTO)
        {
            
            Employee employee = new Employee()
            {

                SSN = EmployeeDTO.SSN,
                Name = EmployeeDTO.Name,
                Nationality = EmployeeDTO.Nationality,
                Gender = EmployeeDTO.Gender,
                phone = EmployeeDTO.phone,
                City = EmployeeDTO.City,
                Country = EmployeeDTO.Country,
                street = EmployeeDTO.street,
                Salary = EmployeeDTO.Salary,
                HireDate = EmployeeDTO.HireDate.AddDays(1),
                BirthDate = EmployeeDTO.BirthDate.AddDays(1),
                LeaveTime  = EmployeeDTO.LeaveTime,
                AttendTime = EmployeeDTO.AttendTime,
                Dept_id = EmployeeDTO.Dept_id
            };
            db.Employees.Add(employee);
            return employee;
            
        }

        public void Save() 
        {
            db.SaveChanges();
        }

        public Employee GetEmployee(int id) 
        {
            return db.Employees.Include(e => e.Department).FirstOrDefault(n => n.Id == id);
        }

        public Employee Edit(AddEmployeeDTO EmployeeDTO, int id)
        {
            
            Employee employee = GetEmployee(id);
            employee.Id = id;
            employee.SSN = EmployeeDTO.SSN;
            employee.Name = EmployeeDTO.Name;
            employee.Nationality = EmployeeDTO.Nationality;
            employee.Gender = EmployeeDTO.Gender;
            employee.phone = EmployeeDTO.phone;
            employee.City = EmployeeDTO.City;
            employee.Country = EmployeeDTO.Country;
            employee.street = EmployeeDTO.street;
            employee.Salary = EmployeeDTO.Salary;
            employee.HireDate = EmployeeDTO.HireDate.AddDays(1); ;
            employee.BirthDate = EmployeeDTO.BirthDate.AddDays(1); ;
            employee.LeaveTime = EmployeeDTO.LeaveTime;
            employee.AttendTime = EmployeeDTO.AttendTime;
            employee.Dept_id = EmployeeDTO.Dept_id;
            

            return employee;
            
        }

        
        public Employee Delete(int id)
        { 
            Employee employee = GetEmployee(id);
            db.Employees.Remove(employee);
            return employee;
        }

        public List<Employee> GetAll()
        {
            return db.Employees.Include(e=>e.Department).ToList();

        }
        public List<Employee> GetEmployeeByName(string name) 
        {
            return db.Employees.Include(e => e.Department).Where(e=>e.Name==name).ToList();
        }

        public List<Employee> GetEmployeesStartingWith(string searchTerm)
        {
            using (var context = new ITIContext())
            {
                var employees = context.Employees.Include(e=>e.Department)
                    .Where(e => e.Name.StartsWith(searchTerm))
                    .Take(10)
                    .ToList();

                return employees;
            }
        }

    }
}
