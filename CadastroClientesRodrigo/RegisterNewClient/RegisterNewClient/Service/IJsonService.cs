using RegisterNewClient.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace RegisterNewClient.Service
{
    public interface IJsonService
    {
        int WriteJson(string json);
        string GetPath();
        List<Person> GenerateJsonText(string path);
        void SetJson(string path);

    }
}
