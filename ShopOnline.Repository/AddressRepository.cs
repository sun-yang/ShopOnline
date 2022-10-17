using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Repository
{
    public class AddressRepository : IAddressRepository
    {
        private readonly DataContext dbContext;
        private readonly IAuthRepository authRepository;

        public AddressRepository(DataContext dbContext,
            IAuthRepository authRepository)
        {
            this.dbContext = dbContext;
            this.authRepository = authRepository;
        }
        public async Task<ServiceResponse<Address>> AddOrUpdateAddressAsync(Address address)
        {
            var response = new ServiceResponse<Address>();
            var dbAddress = (await GetAddressAsync()).Data;
            if (dbAddress == null)
            {
                address.UserId = authRepository.GetUserId();
                dbContext.Addresses.Add(address);
                response.Data = address;
            }
            else
            {
                dbAddress.FirstName = address.FirstName;
                dbAddress.LastName = address.LastName;
                dbAddress.State = address.State;
                dbAddress.Country = address.Country;
                dbAddress.City = address.City;
                dbAddress.PostCode = address.PostCode;
                dbAddress.Street = address.Street;
                response.Data = dbAddress;
            }

            await dbContext.SaveChangesAsync();

            return response;
        }

        public async Task<ServiceResponse<Address>> GetAddressAsync()
        {
            var address = await dbContext.Addresses.FirstOrDefaultAsync(
                a => a.UserId == authRepository.GetUserId());
            return new ServiceResponse<Address> { Data = address };
        }
    }
}
