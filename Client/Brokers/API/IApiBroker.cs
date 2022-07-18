using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SneakersBase.Client.Brokers.API
{
    public partial interface IApiBroker
    {
        Task<T> GetAsync<T>(string relativeUrl);
        Task PostAsync<T>(string relativeUrl, T content);
    }
}
