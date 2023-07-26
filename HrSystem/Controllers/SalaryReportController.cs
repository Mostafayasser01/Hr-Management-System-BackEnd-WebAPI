using HrSystem.DTO;
using HrSystem.Models;
using HrSystem.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HrSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryReportController : ControllerBase
    {
        ILeaveAttendRepository AttendanceRepository;
        ISalaryReportRepository SalaryreportRepository;
        IEmployeeRepository employeeRepository;
        public SalaryReportController(ILeaveAttendRepository attendRepository, ISalaryReportRepository reportRepository,IEmployeeRepository Repository)
        {
            AttendanceRepository = attendRepository;
            SalaryreportRepository = reportRepository;
            employeeRepository = Repository;
        }

        //[HttpGet("{name}")]
        [HttpGet("{name}/{startDate}/{endDate}")]
        public ActionResult Get(string name , DateTime startDate, DateTime endDate)
        {
            SalaryReportDTO salaryReportDTO = new SalaryReportDTO();
            Employee employee = SalaryreportRepository.GetEmployee(name);
            salaryReportDTO.EmployeeName = employee.Name;
            salaryReportDTO.DepartName = employee.Department.Name;
            salaryReportDTO.Salary = employee.Salary;


            TimeSpan timeSpan = endDate - startDate;
            int DaysOnmonth = timeSpan.Days;


            int WeekendHolidays = SalaryreportRepository.CalculateWeekendHolidays(startDate, endDate);
            int numberOfDaysOff = SalaryreportRepository.GetHolidays(startDate, endDate);
            List<LeaveAttend> leaveAttends = AttendanceRepository.GetByDateEmp(name,startDate,endDate);
            salaryReportDTO.AttendanceDays = leaveAttends.Count;
            if (DaysOnmonth - leaveAttends.Count - numberOfDaysOff - WeekendHolidays > 0)
            {
                salaryReportDTO.AbsentDays = DaysOnmonth - leaveAttends.Count - numberOfDaysOff - WeekendHolidays;
            }
            else 
            {
                salaryReportDTO.AbsentDays = 0;
            }

            
            salaryReportDTO.BonusCount=SalaryreportRepository.GetBounsByEmpName(name, startDate, endDate);
            salaryReportDTO.BonusCount = Math.Round(salaryReportDTO.BonusCount,3);

            salaryReportDTO.DiscoundCount = SalaryreportRepository.GetDiscountByEmpName(name, startDate, endDate);
            salaryReportDTO.DiscoundCount = Math.Round(salaryReportDTO.DiscoundCount, 3);


            salaryReportDTO.TotalBonus = SalaryreportRepository.GetTotalBounsByEmpName(name, startDate, endDate);
            salaryReportDTO.TotalBonus = Math.Round(salaryReportDTO.TotalBonus, 3);

            salaryReportDTO.TotalDiscound = SalaryreportRepository.GetTotalDiscountByEmpName(name, startDate, endDate);
            salaryReportDTO.TotalDiscound = Math.Round(salaryReportDTO.TotalDiscound, 3);

            salaryReportDTO.TotalSalary = salaryReportDTO.Salary + salaryReportDTO.TotalBonus - salaryReportDTO.TotalDiscound - salaryReportDTO.Salary/30 * salaryReportDTO.AbsentDays;
            salaryReportDTO.TotalSalary = Math.Round(salaryReportDTO.TotalSalary, 3);

            return Ok(salaryReportDTO);
        }

        [HttpGet("{name}")]
        public ActionResult GetEmployeeByName(string name)
        {
            var employees = employeeRepository.GetEmployeesStartingWith(name);


            List<GetEmployeeDTO> EmployeesDTOs = new List<GetEmployeeDTO>();

            foreach (var item in employees)
            {
                GetEmployeeDTO EmployeeDTO = new GetEmployeeDTO()
                {
                    Id = item.Id,
                    SSN = item.SSN,
                    Name = item.Name,
                    Nationality = item.Nationality,
                    Gender = item.Gender,
                    phone = item.phone,
                    City = item.City,
                    Country = item.Country,
                    street = item.street,
                    Salary = item.Salary,
                    HireDate = item.HireDate,
                    BirthDate = item.BirthDate,
                    LeaveTime = item.LeaveTime,
                    AttendTime = item.AttendTime,
                    DeptName = item.Department.Name
                };
                EmployeesDTOs.Add(EmployeeDTO);
            }
            if (EmployeesDTOs == null)
            {
                return NotFound();

            }
            return Ok(EmployeesDTOs);
        }
    }
}
