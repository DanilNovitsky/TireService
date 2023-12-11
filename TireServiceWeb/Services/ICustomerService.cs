namespace TireServiceWeb.Services
{
    public interface ICustomerService
    {
        Task<bool> IsEmailUniqueAsync(string email);
        Task<bool> IsPhoneNumberUniqueAsync(string phoneNumber);
        Task<bool> IsEmailUniqueAsync(string email, int customerId);
        Task<bool> IsPhoneNumberUniqueAsync(string phoneNumber, int customerId);
    }
}
