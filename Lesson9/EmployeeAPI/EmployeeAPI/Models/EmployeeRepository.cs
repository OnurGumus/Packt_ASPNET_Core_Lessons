using EmployeeAPI.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeAPI.Models
{
    public class EmployeeRepository : IEmployeeRepository
    {
        //internal data
        private static readonly ConcurrentDictionary<int, Employee> employees =
            new ConcurrentDictionary<int, Employee>();

        //id generator
        static int currentId = 2;

        //initial values
        static EmployeeRepository()
        {
            employees.TryAdd(1, new Employee
            {
                FirstName = "Mugil",
                LastName = "Ragu",
                Department = "Finance",
                Id = 1
            });
            employees.TryAdd(2, new Employee
            {
                FirstName = "John",
                LastName = "Skeet",
                Department = "IT",
                Id = 2
            });
        }


        public IEnumerable<Employee> GetAllEmployees() => employees.Values;


        public void AddEmployee(Employee employee)
        {
            //in case same employee objected added twice
            //assumption id will not be set else where.
            lock (employee)
            {
                if (employee.Id == 0)
                {
                    employee.Id = Interlocked.Increment(ref currentId);
                }
            }
            employees.TryAdd(employee.Id, employee);
        }

        public Employee GetEmployee(int id)
        {
            employees.TryGetValue(id, out var e);
            return e;
        }

        ///Finds the employee 
        public void RemoveEmployee(int id) =>
            employees.TryRemove(id, out var _);
        /// Finds an employee with same id in collection and copy the relevant properties.
        public void UpdateEmployee(Employee employee)
        {
            //as long as the item is not removed it will try to update
            while (employees.TryGetValue(employee.Id, out var current))
            {
                if (employees.TryUpdate(employee.Id, employee, current))
                {
                    return;
                }
            }
        }
    }

}
