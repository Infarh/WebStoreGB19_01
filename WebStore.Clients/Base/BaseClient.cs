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
        protected readonly HttpClient Client;

        public string ServiceAddress { get; set; }

        protected BaseClient(IConfiguration configuration)
        {
            Client = new HttpClient()
            {
                BaseAddress = new Uri(configuration["ClientAddress"])
            };

            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        protected T Get<T>(string url) where T : new() => GetAsync<T>(url).Result;

        protected async Task<T> GetAsync<T>(string url, CancellationToken cancel = default(CancellationToken))
            where T : new()
        {
            var response = await Client.GetAsync(url, cancel);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content
                    .ReadAsAsync<T>(cancel)
                    .ConfigureAwait(false);
            }

            return new T();
        }

        protected HttpResponseMessage Post<T>(string url, T value) where T : new() => PostAsync(url, value).Result;

        protected async Task<HttpResponseMessage> PostAsync<T>(string url, T value,
            CancellationToken cancel = default(CancellationToken))
        {
            var response = await Client
                .PostAsJsonAsync(url, value, cancel)
                .ConfigureAwait(false);

            return response.EnsureSuccessStatusCode();
        }

        protected HttpResponseMessage Put<T>(string url, T value) where T : new() => PutAsync(url, value).Result;

        protected async Task<HttpResponseMessage> PutAsync<T>(string url, T value,
            CancellationToken cancel = default(CancellationToken))
            where T : new() =>
            (await Client.PutAsJsonAsync(url, value, cancel)
                .ConfigureAwait(false))
            .EnsureSuccessStatusCode();

        protected HttpResponseMessage Delete(string url) => DeleteAsync(url).Result;

        private async Task<HttpResponseMessage> DeleteAsync(string url,
            CancellationToken cancel = default(CancellationToken)) =>
            await Client
                .DeleteAsync(url, cancel)
                .ConfigureAwait(false);

        public void Dispose() => Client.Dispose();
    }
}