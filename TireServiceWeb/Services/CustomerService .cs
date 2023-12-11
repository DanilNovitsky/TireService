using Microsoft.EntityFrameworkCore;
using TireServiceWeb.Context;

namespace TireServiceWeb.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _db;

        public CustomerService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return !await _db.Customers.AnyAsync(x => x.Email == email);
        }

        public async Task<bool> IsEmailUniqueAsync(string email, int customerId)
        {
            return !await _db.Customers.AnyAsync(x => x.Id != customerId && x.Email == email);
        }

        public async Task<bool> IsPhoneNumberUniqueAsync(string phoneNumber)
        {
            return !await _db.Customers.AnyAsync(x => x.PhoneNumber == phoneNumber);
        }

        public async Task<bool> IsPhoneNumberUniqueAsync(string phoneNumber, int customerId)
        {
            return !await _db.Customers.AnyAsync(x => x.Id != customerId && x.PhoneNumber == phoneNumber);
        }
    }
}
