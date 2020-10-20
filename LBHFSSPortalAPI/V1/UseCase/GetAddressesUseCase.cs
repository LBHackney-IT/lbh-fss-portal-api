using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Exceptions;
using LBHFSSPortalAPI.V1.Factories;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LBHFSSPortalAPI.V1.UseCase
{
    public class GetAddressesUseCase : IGetAddressesUseCase
    {
        private readonly IAddressSearchGateway _addressSearchGateway;

        public GetAddressesUseCase(IAddressSearchGateway addressSearchGateway)
        {
            _addressSearchGateway = addressSearchGateway;
        }

        public async Task<List<AddressResponse>> Execute(string postcode)
        {
            var addresses = await _addressSearchGateway.SearchAddresses(postcode).ConfigureAwait(false);

            if (addresses == null)
                throw new UseCaseException()
                {
                    UserErrorMessage = $"Could not retrieve addresses using the supplied postcode"
                };
            var addressResponse = addresses.Select(a => new AddressResponse()
            {
                UPRN = a.UPRN,
                Latitude = a.Latitude,
                Longitude = a.Longitude,
                PostalCode = a.Postcode,
                City = a.Town,
                Country = "United Kingdom",
                StateProvince = a.Locality,
                Address1 = a.Line1 + " " + a.Line2,
                Address2 = a.Line3 + " " + a.Line4
            }).ToList();
            return addressResponse;
        }
    }
}
