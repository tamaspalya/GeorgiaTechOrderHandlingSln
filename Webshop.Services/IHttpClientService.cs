using System;
using Webshop.Services.CatalogService.Models;

namespace Webshop.Services
{
	public interface IHttpClientService
	{
        Task<T> GetAsync<T>(string url);
        Task<HttpResponseMessage> PostAsync(string url, HttpContent content);
        Task<T> UpdateAsync<T>(string url, T data);
    }
}

