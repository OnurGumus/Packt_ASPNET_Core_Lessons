using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EmployeeAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmployeeAPI.Controllers
{
    [Route("api/[controller]")]
    public class EmployeeController : Controller
    {
        readonly IEmployeeRepository employeeRepository;
        public EmployeeController(IEmployeeRepository employeesRepo) =>
            employeeRepository = employeesRepo;


        public IEnumerable<Employee> GetAll() =>        
            employeeRepository.GetAllEmployees();


        [HttpGet("{id}", Name = "GetEmployee")]
        public IActionResult GetById(int id)
        {
            var employee = employeeRepository.GetEmployee(id);
            if (employee == null)
            {
                return NotFound();
            }
            return new ObjectResult(employee);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Employee emp)
        {
            if (emp == null)
            {
                return BadRequest();
            }
            employeeRepository.AddEmployee(emp);
            return CreatedAtRoute("GetEmployee", new { id = emp.Id }, emp);
        }

        [HttpPut]
        public IActionResult Update([FromBody] Employee emp)
        {
            if (emp == null)
            {
                return BadRequest();
            }
            var employee = employeeRepository.GetEmployee(emp.Id);
            if (employee == null)
            {
                return NotFound();
            }
            employeeRepository.UpdateEmployee(emp);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public void Delete(int id) =>
            employeeRepository.RemoveEmployee(id);





    }
}
