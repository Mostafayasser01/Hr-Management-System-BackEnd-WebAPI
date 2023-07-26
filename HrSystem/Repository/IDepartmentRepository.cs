using HrSystem.Models;

namespace HrSystem.Repository
{
    public interface IDepartmentRepository
    {

        List<Department> GetAll();
        Department GetById(int id);
        Department GetByName(string name);

    }
}
