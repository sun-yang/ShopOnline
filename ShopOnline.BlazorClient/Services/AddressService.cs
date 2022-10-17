namespace ShopOnline.BlazorClient.Services
{
    public class AddressService : IAddressService
    {
        private readonly HttpClient httpClient;

        public AddressService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<Address> AddOrUpdateAddressAsync(Address address)
        {
            var response = await httpClient.PostAsJsonAsync("api/address", address);
            return response.Content.ReadFromJsonAsync<ServiceResponse<Address>>().Result.Data;
        }

        public async Task<Address> GetAddressAsync()
        {
            var response = await httpClient.GetFromJsonAsync<ServiceResponse<Address>>("api/address");
            return response.Data;
        }
    }
}
