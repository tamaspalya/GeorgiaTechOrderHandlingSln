using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Service
{
    public interface IHttpClientService
    {
        Task<T> GetAsync<T>(string url);
        Task<TOut> PostAsync<TIn, TOut>(string url, TIn content);
        Task<TOut> UpdateAsync<TIn, TOut>(string url, TIn data);
    }
}
