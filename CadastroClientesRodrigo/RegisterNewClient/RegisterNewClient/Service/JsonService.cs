using Newtonsoft.Json;
using RegisterNewClient.Database;
using RegisterNewClient.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RegisterNewClient.Service
{
    class JsonService:IJsonService
    {
        private readonly JsonContext Json;

        public JsonService(JsonContext json)
        {
            Json = json;


        }

        public string GetPath()
        {
            return Json.GetJsonPath();
        }


        public int WriteJson(string json)
        {
            string path = Json.GetJsonPath();
            var file = File.Open(path, FileMode.Create, FileAccess.Write);
            using (var strm = new StreamWriter(file))
            {
                try
                {
                    strm.Write(json);
                }
                catch (Exception ex)
                {
                    return -1;
                }
            }
            return 1;
        }



        public void SetJson(string path)
        {
            Json.SetJsonPath(path);
        }

        public List<Person> GenerateJsonText(string path)
        {

            var file = File.Open(path, FileMode.Open, FileAccess.ReadWrite);
            List<Person> People;
            using (StreamReader read = new StreamReader(file))
            {
                string json = read.ReadToEnd();
                People = JsonConvert.DeserializeObject<List<Person>>(json);
            }

            return People;
        }


    }
}
