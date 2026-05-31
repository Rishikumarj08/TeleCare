namespace TeleCare.Repository.Interface
{
    using TeleCare.Model;
 
    public interface IPayerRepository
    {
        Task<List<Payer>> GetAllPayersAsync();
        Task<Payer?> GetPayerByIdAsync(int payerId);
    }
}
 
 
 