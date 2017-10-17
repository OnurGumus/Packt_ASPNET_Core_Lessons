﻿using MVCEF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCEF.ViewModels
{
    public class EmployeeAddViewModel
    {
        public List<Employee> EmployeesList { get; set; }
        [Required(ErrorMessage = "Employee Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Employee Designation is required")]
        [MinLength(5, ErrorMessage = "Minimum length of designation should be 5 characters")]
        [TwoWordsValidationAttribute]
        public string Designation { get; set; }

        [Required]
        [Range(1000, 9999.99)]
        public decimal Salary { get; set; }
    }


}
