using RegisterNewClient.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RegisterNewClient.Infrastructure
{
    public interface IRestService
    {
        Task<int> RefreshDataAsync();

        Task<int> SendPeopleAsync(IQueryable<Person> people);

        Task<int> DeleteAllPeopleAsync();

        Task<string> GetRev(HttpResponseMessage message);



    }
}
