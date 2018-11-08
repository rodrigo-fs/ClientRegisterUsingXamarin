using System;
using System.Collections.Generic;
using System.Text;

namespace RegisterNewClient.Json
{
    class JsonContext

    {
        public static string JsonPath { get; set; }

        public JsonContext() { }

        public JsonContext(string jsonPath)
        {
            JsonPath = jsonPath;
        }

        public string GetJsonPath()
        {
            return JsonPath;

        }
        public void SetJsonPath(string path)
        {
            JsonPath = path;
        } 

    }
}
