namespace Customers.Core.DTOs
{
    public class CreateCustomerDto
    {
        public long UserId { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public string? Preferences { get; set; }
    }
}
