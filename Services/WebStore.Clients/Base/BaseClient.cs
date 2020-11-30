using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace WebStore.Clients.Base
{
    public abstract class BaseClient : IDisposable
    {
        protected readonly string _serviceAddress;
        protected readonly HttpClient _client;

        protected BaseClient(IConfiguration Configuration, string serviceAddress)
        {
            _serviceAddress = serviceAddress;
            _client = new HttpClient
            {
                BaseAddress = new Uri(Configuration["WebApiURL"]),
                DefaultRequestHeaders =
                {
                    Accept = { new MediaTypeWithQualityHeaderValue("application/json") }
                }
            };
        }

        public T Get<T>(string url) => GetAsync<T>(url).Result;
        public async Task<T> GetAsync<T>(string url, CancellationToken Cancel = default)
        {
            var response = await _client.GetAsync(url);
            return await response.EnsureSuccessStatusCode().Content.ReadAsAsync<T>();
        }

        public HttpResponseMessage Post<T>(string url, T item) => PostAsync(url, item).Result;
        public async Task<HttpResponseMessage> PostAsync<T>(string url, T item, CancellationToken Cancel = default)
        {
            var response = await _client.PostAsJsonAsync(url, item);
            return response.EnsureSuccessStatusCode();
        }

        public HttpResponseMessage Put<T>(string url, T item) => PutAsync(url, item).Result;
        public async Task<HttpResponseMessage> PutAsync<T>(string url, T item, CancellationToken Cancel = default)
        {
            var response = await _client.PutAsJsonAsync(url, item);
            return response.EnsureSuccessStatusCode();
        }

        public HttpResponseMessage Delete(string url) => DeleteAsync(url).Result;
        public async Task<HttpResponseMessage> DeleteAsync(string url, CancellationToken Cancel = default)
        {
            var response = await _client.DeleteAsync(url);
            return response;
        }

        public void Dispose() => Dispose(true);
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            //_Client.Dispose();
        }
    }
}