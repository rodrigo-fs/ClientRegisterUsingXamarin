using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RegisterNewClient.Database;
using RegisterNewClient.Infrastructure;
using RegisterNewClient.TodoREST;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Drawing;

namespace RegisterNewClient.Service
{
    public class RestService : IRestService
    {
        HttpClient Server;
        IPersonService PersonService;
        IFileSystem FileSystem;
        public List<Person> Persons { get; private set; }

        public RestService(IPersonService personService,IFileSystem fileSytem )
        {
            FileSystem = fileSytem;
            PersonService = personService;
            var authData = string.Format("{0}:{1}", Constants.Username, Constants.Password);
            var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));

            Server = new HttpClient();
            //Server.MaxResponseContentBufferSize = 256000;
            Server.DefaultRequestHeaders.Add("Connection", "keep-alive");
            Server.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);



        }

        public async Task<string> GetRev(HttpResponseMessage message)
        {
            string JsonResponse = await message.Content.ReadAsStringAsync();
            JToken joResponse= JObject.Parse(JsonResponse)["_rev"];
            if (joResponse == null)
                joResponse = JObject.Parse(JsonResponse)["rev"];
            string rev = joResponse.Value<string>();
            return rev;




        }


        public async Task<int> DeleteAllPeopleAsync()
        {
            HttpResponseMessage response = null;

            var ret = -1;
            var uri = string.Format(Constants.RestUrl, string.Empty);
            response = await Server.GetAsync(string.Concat(uri, "/_all_docs?include_docs=true"));

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var jobj = JObject.Parse(jsonResponse.ToString());
            IList<JToken> people = jobj["rows"].Children().ToList();
            try
            {
                foreach (JToken person in people)
                {
                    response = await Server.DeleteAsync(string.Concat(uri, $"/{int.Parse(person["doc"]["_id"].ToString())}?rev={person["value"]["rev"].ToString()}"));
                    ret = 1;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"				ERROR {0}", ex.Message);
                return 0;

            }
            return ret;
        }

        public async Task<int> RefreshDataAsync()
        {

            var storagePath = FileSystem.GetStoragePathToImage();
            var uri = new Uri(string.Format(Constants.RestUrl, string.Empty));
            var ret = -1;
            try
            {
                
                var response = await Server.GetAsync(string.Concat(uri,"/_all_docs?include_docs=true"));
                
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var jobj = JObject.Parse(jsonResponse.ToString());
                IList<JToken> people = jobj["rows"].Children().ToList();
                foreach(JToken person in people)
                {
                    var aux = new Person { PersonId = int.Parse(person["doc"]["_id"].ToString()), Name = person["doc"]["Name"].ToString(), Age = int.Parse(person["doc"]["Age"].ToString()), Telephone = person["doc"]["Telephone"].ToString() };
                    var ifUserExist = PersonService.GetPersons(aux.PersonId.ToString(), 0);
                    if (ifUserExist.Count() > 0)
                    {
                        PersonService.DeletePersons(aux.PersonId.ToString(), 0);
                    }
                    var validator = await Server.GetAsync(string.Concat(uri, $"/{aux.PersonId}/userImage.png"));
                    if (validator.IsSuccessStatusCode)
                    {
                        var content = await validator.Content.ReadAsByteArrayAsync();
                        var pathImage = Path.Combine(storagePath, $"userImage{aux.PersonId}.png");
                        if(File.Exists(pathImage))
                        {
                            File.Delete(pathImage);
                        }
                        File.WriteAllBytes(pathImage, content);
                        aux.ImagePath = pathImage;
                       
                    }
                    else
                    {
                        aux.ImagePath = "userIcon.png";
                    }
                   
                    PersonService.AddPersons(aux);

                    ret = 1;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"                  ERROR {0}", e.Message);
                return 0;
            }

            return ret;
        }


        public async Task<int> SendPeopleAsync(IQueryable<Person> people)
        {
            HttpResponseMessage response = null;
            ByteArrayContent imagePackage;

            var uri = string.Format(Constants.RestUrl, string.Empty);
            string rev;
            try
            {
                foreach (Person person in people)
                {
                    imagePackage = null;
                    var json = new
                    {
                        
                        _id = person.PersonId.ToString(),
                        Name = person.Name,
                        Age = person.Age,
                        Telephone = person.Telephone,


                    };
                    var body = new StringContent(JsonConvert.SerializeObject(json), Encoding.UTF8, "application/json");

                    if (!person.ImagePath.Contains("userIcon.png"))
                    {
                        try
                        {
                            imagePackage = new ByteArrayContent(System.IO.File.ReadAllBytes(person.ImagePath));
                            imagePackage.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                        }
                        catch(Exception e)
                        {
                            Debug.Write(e.Message);
                        }
                    }
                    /*else
                    {
                        imagePackage = null;
                    }*/
                    var verify = await Server.GetAsync(String.Concat(uri, $"/{person.PersonId}"));
                    var jsonResponse = await verify.Content.ReadAsStringAsync();
                    var joResponse = JObject.Parse(jsonResponse)["_id"];
                    if (joResponse == null)
                    {
                        response = await Server.PostAsync(uri, body);
                        if (imagePackage != null )
                        {
                            rev = await GetRev(response);
                            response = await Server.PutAsync(string.Concat(uri, $"/{person.PersonId}/userImage.png?rev={rev}"), imagePackage);
                        }
                    }
                    else
                    {
                        rev = await GetRev(verify);
                        response = await Server.PutAsync(string.Concat(uri, $"/{person.PersonId}?rev={rev}"), body);
                        if (imagePackage != null)
                        {
                            rev = await GetRev(response);
                            response = await Server.PutAsync(string.Concat(uri, $"/{person.PersonId}/userImage.png?rev={rev}"), imagePackage);
                        }
                    }
                }

            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine(e.InnerException.Message);
                return 0;
            }
            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine(@"                                          Person sucessfully saved");
                return 1;
            }
            return 0;
        }
    }
}

           
          