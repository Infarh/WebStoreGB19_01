using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
// ReSharper disable SettingNotFoundInConfiguration
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable RedundantAwait

namespace WebStore.Clients.Base
{
    public abstract class BaseClient : IDisposable
    {
        protected readonly HttpClient _Client;

        public string ServiceAddress { get; set; }

        protected BaseClient(IConfiguration configuration)
        {
            _Client = new HttpClient
            {
                BaseAddress = new Uri(configuration["ClientAddress"])
            };
            _Client.DefaultRequestHeaders.Accept.Clear();
            _Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        protected T Get<T>(string url) where T : new() => GetAsync<T>(url).Result;

        protected async Task<T> GetAsync<T>(string url, CancellationToken cancel = default(CancellationToken)) 
            where T : new()
        {
            var response = await _Client.GetAsync(url, cancel);
            if (response.IsSuccessStatusCode)
                return await response.Content
                    .ReadAsAsync<T>(cancel)
                    .ConfigureAwait(false);
            return new T();
        }

        protected HttpResponseMessage Post<T>(string url, T value) => PostAsync(url, value).Result;

        protected async Task<HttpResponseMessage> PostAsync<T>(
            string url, 
            T value, 
            CancellationToken cancel = default(CancellationToken))
        {
            var response = await _Client
                .PostAsJsonAsync(url, value, cancel)
                .ConfigureAwait(false);
            return response.EnsureSuccessStatusCode();
        }

        protected HttpResponseMessage Put<T>(string url, T value)
            => PutAsync(url, value).Result;

        protected async Task<HttpResponseMessage> PutAsync<T>(
            string url,
            T value,
            CancellationToken cancel = default(CancellationToken)) =>
                (await _Client.PutAsJsonAsync(url, value, cancel)
                    .ConfigureAwait(false))
                    .EnsureSuccessStatusCode();

        protected HttpResponseMessage Delete(string url) => DeleteAsync(url).Result;

        protected async Task<HttpResponseMessage> DeleteAsync(
            string url,
            CancellationToken cancel = default(CancellationToken)) =>
            await _Client.DeleteAsync(url, cancel)
                .ConfigureAwait(false);

        #region Implementation of IDisposable

        void IDisposable.Dispose() => _Client.Dispose();

        #endregion
    }
}
