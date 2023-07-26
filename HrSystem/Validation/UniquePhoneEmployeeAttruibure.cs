﻿using System.ComponentModel.DataAnnotations;
using HrSystem.DTO;
using HrSystem.Models;

namespace HrSystem.Validation
{
    public class UniquePhoneEmployeeAttruibure : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            //get value
            string Phone = (string)value;
            AddEmployeeDTO empValid = validationContext.ObjectInstance as AddEmployeeDTO;

            ITIContext context = new ITIContext();

            Employee EmpValidDb = context.Employees.FirstOrDefault(e => e.phone == Phone);

            if (EmpValidDb == null)
            {
                return ValidationResult.Success;
            }
            else if (EmpValidDb.Id == empValid.Id)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Phone Number already Found");
        }
    }

}
