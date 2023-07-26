using HrSystem.DTO;
using HrSystem.Models;

namespace HrSystem.Repository
{
    public interface IEmployeeRepository
    {
        public List<Employee> GetAll();
        public Employee Add(AddEmployeeDTO EmployeeDTO);
        public void Save();
        public Employee GetEmployee(int id);
        public List<Employee> GetEmployeeByName(string name);
        public List<Employee> GetEmployeesStartingWith(string searchTerm);
        public Employee Edit(AddEmployeeDTO EmployeeDTO, int id);
        public Employee Delete(int id);
    }
}
