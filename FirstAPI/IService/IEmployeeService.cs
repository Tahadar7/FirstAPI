using FirstAPI.DTO;

namespace FirstAPI.IService
{
    public interface IEmployeeService
    {
        Task<Tuple<int, List<EmployeeDTO>>> AllEmployees();
        Task<Tuple<int, string>> CreateEmployee(EmployeeDTO employeedto);
        Task<Tuple<int, string>> UpdateEmployee(EmployeeDTO employeedto);
        Task<Tuple<int, string>> RemoveEmployee(Guid id);
        Task<Tuple<int, EmployeeDTO>> GetEmployeeById(Guid id);
    }
}
