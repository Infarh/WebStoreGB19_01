using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WebStore.Clients.Base;
using WebStore.Interfaces.Api;

namespace WebStore.Clients.Values
{
    public class ValuesClient : BaseClient, IValuesService
    {
        public ValuesClient(IConfiguration configuration) : base(configuration)
        {
            ServiceAddress = "api/values";
        }


        public IEnumerable<string> Get() => GetAsync().Result;

        public async Task<IEnumerable<string>> GetAsync()
        {
            var result = await _Client.GetAsync(ServiceAddress);
            if (result.IsSuccessStatusCode)
                return await result.Content.ReadAsAsync<List<string>>();
            return Enumerable.Empty<string>();
        }

        public string Get(int id) => GetAsync(id).Result;

        public async Task<string> GetAsync(int id)
        {
            var result = await _Client.GetAsync($"{ServiceAddress}/get/{id}");
            if(result.IsSuccessStatusCode)
                return await result.Content.ReadAsAsync<string>();
            return string.Empty;
        }

        public Uri Post(string value) => PostAsync(value).Result;

        public async Task<Uri> PostAsync(string value)
        {
            var result = await _Client.PostAsJsonAsync($"{ServiceAddress}/post", value);
            result.EnsureSuccessStatusCode();
            return result.Headers.Location;
        }

        public HttpStatusCode Put(int id, string value) => PutAsync(id, value).Result;

        public async Task<HttpStatusCode> PutAsync(int id, string value)
        {
            var result = await _Client.PutAsJsonAsync($"{ServiceAddress}/put/{id}", value);
            result.EnsureSuccessStatusCode();
            return result.StatusCode;
        }

        public HttpStatusCode Delete(int id) => DeleteAsync(id).Result;

        public async Task<HttpStatusCode> DeleteAsync(int id)
        {
            var result = await _Client.DeleteAsync($"{ServiceAddress}/delete/{id}");
            return result.StatusCode;
        }
    }
}
