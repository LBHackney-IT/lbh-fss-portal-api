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
    public class AddressXRefGateway : IAddressXRefGateway
    {
        private readonly HttpClient _client;

        public AddressXRefGateway(HttpClient client)
        {
            _client = client;
        }

        public string GetNHSNeighbourhood(string uprn)
        {
            var addressXRefs = GetAddressXrefs(uprn).Result;
            if ((addressXRefs != null) && (addressXRefs.Count > 0))
            {
                var addressXRef = addressXRefs.Where(x => x.Code == "5360NN").FirstOrDefault();
                return addressXRef == null ? string.Empty : addressXRef.Value;
            }
            return string.Empty;
        }

        public async Task<List<AddressXRef>> GetAddressXrefs(string uprn)
        {
            try
            {
                var response = await _client.GetAsync(new Uri($"properties/{uprn}/crossreferences", UriKind.Relative)).ConfigureAwait(true);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
                }
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                var addressXRefDomain = JsonConvert.DeserializeObject<AddressXRefAPIResponse>(content);
                var addressXRef = addressXRefDomain.Data.AddressCrossReferences;
                if (addressXRef != null)
                    return addressXRef;
                return null;
            }
            catch (Exception e)
            {
                LoggingHandler.LogError(e.Message);
                LoggingHandler.LogError(e.StackTrace);
                return null;
            }
        }
    }
}
