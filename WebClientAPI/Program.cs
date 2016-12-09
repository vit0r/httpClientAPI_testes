using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebClientAPI
{
    class Program
    {
        protected const string ADDRESS_API = "https://jsonplaceholder.typicode.com/";
        protected const string RESOURCE_VALUES = "todos";

        static void Main(string[] args)
        {
            //RequestAPI();

            List<User> users = ExecuteGet<List<User>>(ADDRESS_API, RESOURCE_VALUES);
            users.ForEach(u => Console.WriteLine(string.Format("{0} - {1} - {2} - {3}", u.Id, u.UserId, u.Title, u.Complete)));

            Post putPost = new Post { UserId = 1, Id = 1, Title = "teste 1", Body = "content post" };
            Post post = ExecutePut<Post>("http://localhost:5000/", "posts/"+putPost.Id, putPost);

            Console.ReadLine();
        }

        private static void RequestAPI()
        {
            try
            {
                WebClient client = new WebClient();
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
                client.DownloadStringAsync(new Uri(string.Concat(ADDRESS_API, RESOURCE_VALUES)));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                List<User> users = JsonConvert.DeserializeObject<List<User>>(e.Result);
                Console.WriteLine(users.Count);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static T ExecuteGet<T>(string baseUrl, string resourceName, int timeoutMinutes = 5)
        {
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(timeoutMinutes);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string uri = string.Format("{0}{1}", baseUrl, resourceName);
            Task<HttpResponseMessage> response = client.GetAsync(uri, HttpCompletionOption.ResponseContentRead);
            while (!response.GetAwaiter().IsCompleted)
            {
                var errorProcess = response.IsCanceled || response.IsFaulted || response.Exception != null;
                if (errorProcess)
                {
                    break;
                }
            }
            if (response.Result.IsSuccessStatusCode)
            {
                var result = response.Result.Content.ReadAsStringAsync().Result;
                T obj = JsonConvert.DeserializeObject<T>(result);
                return obj;
            }
            else
            {
                Console.WriteLine(response.Result.StatusCode.ToString());
            }
            throw new ArgumentException("Parser error");
        }

        private static T ExecutePut<T>(string baseUrl, string resourceName, object body, int timeoutMinutes = 5)
        {
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(timeoutMinutes);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string uri = string.Format("{0}{1}", baseUrl, resourceName);
            string json = JsonConvert.SerializeObject(body);
            HttpContent httpContent = new StringContent(json);
            httpContent.Headers.ContentType.MediaType = "application/json";
            Task<HttpResponseMessage> response = client.PutAsync(uri, httpContent);
            while (!response.GetAwaiter().IsCompleted)
            {
                var errorProcess = response.IsCanceled || response.IsFaulted || response.Exception != null;
                if (errorProcess)
                {
                    break;
                }
            }
            if (response.Result.IsSuccessStatusCode)
            {
                var result = response.Result.Content.ReadAsStringAsync().Result;
                T obj = JsonConvert.DeserializeObject<T>(result);
                return obj;
            }
            else
            {
                Console.WriteLine(response.Result.StatusCode.ToString());
            }
            throw new ArgumentException("Parser error");
        }
    }
}
