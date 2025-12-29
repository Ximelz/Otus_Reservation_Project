using Customers.Core.Data;
using Customers.Core.DTOs;
using Customers.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Customers.API.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    [Route("/api/[controller]/V1")]
    public class CustomersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(
            AppDbContext context,
            ILogger<CustomersController> logger)
        {
            _context = context;
            _logger = logger;
        }


        // GET: api/customers
        // GET: api/customers?userId=123
        // GET: api/customers?fullName=John
        // GET: api/customers?phone=+123456789
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers(
            [FromQuery] string? Id = null,
            [FromQuery] long? userId = null,
            [FromQuery] string? fullName = null,
            [FromQuery] string? phone = null)
        {
            try
            {
                // Используем IQueryable<Customer>, а не CustomerDto
                IQueryable<Customer> query = _context.Customers.AsQueryable();

                // Применяем фильтры
                if (!string.IsNullOrWhiteSpace(Id))
                {
                    query = query.Where(c => c.Id == Guid.Parse(Id));
                }

                if (userId.HasValue)
                {
                    query = query.Where(c => c.UserId == userId.Value);
                }

                if (!string.IsNullOrWhiteSpace(fullName))
                {
                    query = query.Where(c => c.FullName.Contains(fullName));
                }

                if (!string.IsNullOrWhiteSpace(phone))
                {
                    query = query.Where(c => c.Phone.Contains(phone));
                }

                var customers = await query
                    .OrderByDescending(c => c.CreatedAt)
                    //.Select(c => MapToDto(c))
                    .Select(c => c)
                    .ToListAsync();

                return Ok(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка клиентов");
                return StatusCode(500, "Произошла ошибка при обработке запроса");
            }
        }

        // GET: api/customers/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomer(Guid id)
        {
            try
            {
                // Ищем сущность Customer
                var customer = await _context.Customers.FindAsync(id);

                if (customer == null)
                {
                    return NotFound($"Клиент с ID {id} не найден");
                }

                //return Ok(MapToDto(customer));
                return Ok(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении клиента по ID {Id}", id);
                return StatusCode(500, "Произошла ошибка при обработке запроса");
            }
        }

        // POST: api/customers
        [HttpPost]
        public async Task<ActionResult<CustomerDto>> CreateCustomer(CreateCustomerDto createDto)
        {
            try
            {
                // Проверка уникальности UserId
                if (await _context.Customers.AnyAsync(c => c.UserId == createDto.UserId))
                {
                    return Conflict($"Клиент с UserId {createDto.UserId} уже существует");
                }

                // Проверка уникальности email
                if (await _context.Customers.AnyAsync(c => c.Email == createDto.Email))
                {
                    return Conflict($"Клиент с email {createDto.Email} уже существует");
                }

                // Создаем сущность Customer из DTO
                var customer = new Customer
                {
                    Id = Guid.NewGuid(),
                    UserId = createDto.UserId,
                    FullName = createDto.FullName,
                    Email = createDto.Email,
                    Phone = createDto.Phone,
                    Preferences = createDto.Preferences,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                // Добавляем сущность в контекст
                await _context.Customers.AddAsync(customer);
                await _context.SaveChangesAsync();

                // Возвращаем DTO
                return CreatedAtAction(
                    nameof(GetCustomer),
                    new { id = customer.Id },
                    //MapToDto(customer));
                    customer);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка при создании клиента");
                return StatusCode(500, "Ошибка при сохранении данных");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании клиента");
                return StatusCode(500, "Произошла ошибка при обработке запроса");
            }
        }

        // PUT: api/customers/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(Guid id, UpdateCustomerDto updateDto)
        {
            try
            {
                // Ищем сущность Customer
                var customer = await _context.Customers.FindAsync(id);

                if (customer == null)
                {
                    return NotFound($"Клиент с ID {id} не найден");
                }

                // Проверка уникальности email (если изменился)
                if (customer.Email != updateDto.Email &&
                    await _context.Customers.AnyAsync(c => c.Email == updateDto.Email))
                {
                    return Conflict($"Клиент с email {updateDto.Email} уже существует");
                }

                // Обновляем поля сущности
                customer.FullName = updateDto.FullName;
                customer.Email = updateDto.Email;
                customer.Phone = updateDto.Phone;
                customer.Preferences = updateDto.Preferences;
                customer.UpdatedAt = DateTime.UtcNow;

                // Обновляем в контексте
                _context.Customers.Update(customer);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении клиента {Id}", id);
                return StatusCode(500, "Ошибка при сохранении данных");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении клиента {Id}", id);
                return StatusCode(500, "Произошла ошибка при обработке запроса");
            }
        }

        // DELETE: api/customers/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            try
            {
                // Ищем сущность Customer
                var customer = await _context.Customers.FindAsync(id);

                if (customer == null)
                {
                    return NotFound($"Клиент с ID {id} не найден");
                }

                // Удаляем сущность
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка при удалении клиента {Id}", id);
                return StatusCode(500, "Ошибка при удалении данных");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении клиента {Id}", id);
                return StatusCode(500, "Произошла ошибка при обработке запроса");
            }
        }




        // Вспомогательный метод для преобразования Customer в CustomerDto
        private CustomerDto MapToDto(Customer customer)
        {
            return new CustomerDto
            {
                Id = customer.Id,
                UserId = customer.UserId,
                FullName = customer.FullName,
                Email = customer.Email,
                Phone = customer.Phone,
                Preferences = customer.Preferences,
                CreatedAt = customer.CreatedAt,
                UpdatedAt = customer.UpdatedAt
            };
        }

    }
}
