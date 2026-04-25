using FirstAPI.Data;
using FirstAPI.DTO;
using FirstAPI.Entities;
using FirstAPI.IService;
using Microsoft.EntityFrameworkCore;

namespace FirstAPI.Services
{
    public class EmployeeService(ApplicationDBContext context) : IEmployeeService
    {
        public async Task<Tuple<int, List<EmployeeDTO>>> AllEmployees()
        {
            try
            {
                var employeeslist = await context.Employees.AsNoTracking().Select(e => new EmployeeDTO
                {
                    Id = e.Id,
                    Name = e.Name,
                    CreatedDate = e.CreatedDate,
                    LastModifiedDate = e.LastModifiedDate,
                    DateOfBirth = e.DateOfBirth,
                    Position = e.Position,
                    Department = e.Department,
                    Email = e.Email,
                }).ToListAsync();

                return new Tuple<int, List<EmployeeDTO>>(1, employeeslist);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Tuple<int, string>> CreateEmployee(EmployeeDTO employeedto)
        {
            try
            {
                if (employeedto == null || string.IsNullOrWhiteSpace(employeedto.Name) || string.IsNullOrWhiteSpace(employeedto.Email))
                {
                    return new Tuple<int, string>(1, "Invalid data! Employee data is null.");  // 1 = Invalid data 
                }

                var existingEmployee = await context.Employees.AnyAsync(e => e.Email == employeedto.Email);

                if (existingEmployee)
                {
                    return new Tuple<int, string>(3, "An employee with the same email already exists.");   // 3 = Duplicate email conflict
                }

                var newEmployee = new Employee
                {
                    Id = Guid.NewGuid(),
                    Name = employeedto.Name,
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = null,
                    DateOfBirth = employeedto.DateOfBirth,
                    Position = employeedto.Position,
                    Department = employeedto.Department,
                    Email = employeedto.Email
                };

                context.Employees.Add(newEmployee);
                await context.SaveChangesAsync();

                return new Tuple<int, string>(2, "Employee created successfully.");   // 2 = Success
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Tuple<int, string>> UpdateEmployee(EmployeeDTO employeedto)
        {
            try
            {
                if (employeedto == null)
                {
                    return new Tuple<int, string>(1, "Invalid data! Employee data is null.");  // 1 = Invalid data 
                }
                var employeeExist = await context.Employees.FirstOrDefaultAsync(e => e.Email == employeedto.Email);

                if (employeeExist == null)
                {
                    return new Tuple<int, string>(0, "Employee not found.");   // 0 = Employee not found    
                }

                employeeExist.LastModifiedDate = DateTime.Now;
                employeeExist.Name = string.IsNullOrWhiteSpace(employeedto.Name) ? employeeExist.Name : employeedto.Name;
                employeeExist.DateOfBirth = employeedto.DateOfBirth ?? employeeExist.DateOfBirth;
                employeeExist.Position = string.IsNullOrWhiteSpace(employeedto.Position) ? employeeExist.Position : employeedto.Position;
                employeeExist.Department = string.IsNullOrWhiteSpace(employeedto.Department) ? employeeExist.Department : employeedto.Department;
                employeeExist.Email = string.IsNullOrWhiteSpace(employeedto.Email) ? employeeExist.Email : employeedto.Email;

                context.Employees.Update(employeeExist);
                await context.SaveChangesAsync();

                return new Tuple<int, string>(2, "Employee updated successfully.");   // 2 = Success
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Tuple<int, string>> RemoveEmployee(Guid id)
        {
            try
            {
                var employeedata = await context.Employees.FirstOrDefaultAsync(e => e.Id == id);
                if (employeedata == null)
                {
                    return new Tuple<int, string>(0, "Employee not found.");   // 0 = Employee not found    
                }
                context.Employees.Remove(employeedata);
                await context.SaveChangesAsync();
                return new Tuple<int, string>(2, "Employee deleted successfully.");   // 2 = Success
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Tuple<int, EmployeeDTO>> GetEmployeeById(Guid id)
        {
            try
            {
                var employee = await context.Employees.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);

                if (employee == null)
                {
                    return new Tuple<int, EmployeeDTO>(0, null);   // 0 = Employee not found
                }

                var employeeDTO = new EmployeeDTO
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    CreatedDate = employee.CreatedDate,
                    LastModifiedDate = employee.LastModifiedDate,
                    DateOfBirth = employee.DateOfBirth,
                    Position = employee.Position,
                    Department = employee.Department,
                    Email = employee.Email
                };
                return new Tuple<int, EmployeeDTO>(2, employeeDTO);   // 2 = Success
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
