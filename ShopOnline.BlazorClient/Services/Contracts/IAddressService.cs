namespace ShopOnline.BlazorClient.Services.Contracts
{
    public interface IAddressService
    {
        Task<Address> GetAddressAsync();
        Task<Address> AddOrUpdateAddressAsync(Address address);
    }
}
