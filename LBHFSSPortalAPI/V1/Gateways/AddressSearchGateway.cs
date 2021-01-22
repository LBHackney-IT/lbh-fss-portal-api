using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Exceptions;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
using LBHFSSPortalAPI.V1.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using LBHFSSPortalAPI.V1.Handlers;

namespace LBHFSSPortalAPI.V1.Gateways
{
    public class AddressSearchGateway : IAddressSearchGateway
    {
        private readonly HttpClient _client;

        public AddressSearchGateway(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<Address>> SearchAddresses(string postCode)
        {
            try
            {
                var response = await _client.GetAsync(new Uri($"addresses/?format=detailed&postcode={postCode}", UriKind.Relative)).ConfigureAwait(true);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
                }
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                var addressDomain = JsonConvert.DeserializeObject<AddressAPIResponse>(content);
                var addresses = addressDomain.Data.Addresses;
                if (addressDomain.Data.PageCount > 1)
                {
                    for (int i = 2; i <= addressDomain.Data.PageCount; i++)
                    {
                        response = await _client.GetAsync(new Uri($"addresses/?format=detailed&postcode={postCode}&page={i}", UriKind.Relative)).ConfigureAwait(true);
                        if (!response.IsSuccessStatusCode)
                        {
                            throw new Exception(response.ReasonPhrase);
                        }
                        content = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                        addressDomain = JsonConvert.DeserializeObject<AddressAPIResponse>(content);
                        addresses = addresses.Concat(addressDomain.Data.Addresses).ToList();
                    }
                }
                if (addresses != null)
                    return addresses;
                return null;
            }
            catch (Exception e)
            {
                LoggingHandler.LogError(e.Message);
                LoggingHandler.LogError(e.StackTrace);
                throw new UseCaseException()
                {
                    UserErrorMessage = "Could not search for addresses",
                    DevErrorMessage = e.Message
                };
            }
        }
    }
}
