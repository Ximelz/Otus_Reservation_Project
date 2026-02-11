using Customers.Core.DTOs;
using System.Threading.Tasks;

namespace Customers.Core.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerDto> GetCustomerAsync(long userId);
        Task<bool> UpdateCustomerAsync(long userId, CustomerDto customerDto);
    }
}
