namespace FirstAPI.DTO
{
    public class EmployeeDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Position { get; set; }
        public string? Department { get; set; }
        public string? Email { get; set; }
    }
}
