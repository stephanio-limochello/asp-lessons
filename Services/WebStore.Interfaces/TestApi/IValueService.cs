using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace WebStore.Interfaces.TestApi
{
    public interface IValueService
    {
        IEnumerable<string> Get();

        string Get(int id);

        Uri Post(string value);
        Task<Uri> PostAsync(string value);

        HttpStatusCode Update(int id, string value);

        HttpStatusCode Delete(int id);
    }
}