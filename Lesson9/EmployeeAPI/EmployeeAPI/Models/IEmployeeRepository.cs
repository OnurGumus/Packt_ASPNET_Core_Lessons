
using System.Collections.Generic;

namespace EmployeeAPI.Models
{
    public interface IEmployeeRepository
    {
        void AddEmployee(Employee employee);
        IEnumerable<Employee> GetAllEmployees();
        Employee GetEmployee(int id);
        void RemoveEmployee(int id);
        void UpdateEmployee(Employee employee);
    }
}
