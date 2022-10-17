using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Repository.Contracts
{
    public interface IAddressRepository
    {
        Task<ServiceResponse<Address>> GetAddressAsync();
        Task<ServiceResponse<Address>> AddOrUpdateAddressAsync(Address address);
    }
}
