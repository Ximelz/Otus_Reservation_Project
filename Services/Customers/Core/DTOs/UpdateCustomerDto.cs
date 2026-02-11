namespace Customers.Core.DTOs
{
    public class UpdateCustomerDto
    {
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public string? Preferences { get; set; }
    }
}
