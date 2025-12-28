using System;
namespace Customers.Core.DTOs
{

    public class CustomerDto
    {
        public Guid Id { get; set; }
        public long UserId { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public string? Preferences { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}