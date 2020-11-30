using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

using WebStore.Clients.Base;
using WebStore.Interfaces.TestApi;

namespace WebStore.Clients.Values
{
    public class ValuesClient : BaseClient, IValueService
    {
        public ValuesClient(IConfiguration Configuration) : base(Configuration, "api/v1/values") { }

        public IEnumerable<string> Get()
        {
            var respone = _client.GetAsync(_serviceAddress).Result;
            if (respone.IsSuccessStatusCode)
                return respone.Content.ReadAsAsync<IEnumerable<string>>().Result;

            return Enumerable.Empty<string>();
        }

        public string Get(int id)
        {
            var respone = _client.GetAsync($"{_serviceAddress}/{id}").Result;
            if (respone.IsSuccessStatusCode)
                return respone.Content.ReadAsAsync<string>().Result;

            return string.Empty;
        }

        public Uri Post(string value)
        {
            var response = _client.PostAsJsonAsync(_serviceAddress, value).Result;
            return response.EnsureSuccessStatusCode().Headers.Location;
        }
        public async Task<Uri> PostAsync(string value)
        {
            var response = await _client.PostAsJsonAsync(_serviceAddress, value);
            var ens = response.EnsureSuccessStatusCode();
            var hedrs = ens.Headers;
            var loc = hedrs.Location;
            return response.EnsureSuccessStatusCode().Headers.Location;
        }

        public HttpStatusCode Update(int id, string value)
        {
            var response = _client.PutAsJsonAsync($"{_serviceAddress}/{id}", value).Result;
            return response.EnsureSuccessStatusCode().StatusCode;
        }

        public HttpStatusCode Delete(int id)
        {
            var response = _client.DeleteAsync($"{_serviceAddress}/{id}").Result;
            return response.StatusCode;
        }
    }
}