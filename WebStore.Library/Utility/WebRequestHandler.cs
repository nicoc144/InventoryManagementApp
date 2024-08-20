using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace WebStore.Library.Utilities
{
    //
    // This is pretty much just boilerplate code for making a web request
    //
    public class WebRequestHandler
    {
        private string host = "localhost";
        private string port = "7206";
        private HttpClient Client { get; }
        public WebRequestHandler()
        {
            Client = new HttpClient();
        }
        public async Task<string> Get(string url) //get, takes in the name of the list ex: "/Inventory" list
        {
            var fullUrl = $"https://{host}:{port}{url}";
            try
            {
                using (var client = new HttpClient()) //Create an instance of http client (for sending http requests)
                                                      //Using statement ensures that the instance is properly disposed of
                                                      //after use
                {
                    var response = await client //await is waiting for the getstringasync method to complete
                        .GetStringAsync(fullUrl) //asyncronous get request to the full url
                        .ConfigureAwait(false);  //says that you can continue running code on any thread, not just on the original thread
                    return response;
                }
            } catch(Exception e)
            {

            }


            return null;
        }

        public async Task<string> Delete(string url) //delete, takes in the id of the item you want to delete
        {
            var fullUrl = $"https://{host}:{port}{url}";
            try
            {
                using (var client = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(HttpMethod.Delete, fullUrl)) //call httprequestmessage with delete function
                    {
                        using (var response = await client
                                .SendAsync(request, HttpCompletionOption.ResponseHeadersRead) //operation should complete when response headers read
                                .ConfigureAwait(false))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                return await response.Content.ReadAsStringAsync();
                            }
                            return "ERROR";
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }


            return null;
        }

        public async Task<string> Post(string url, object obj) //post, takes in the inventory list you want to post to, and the item you would like to add/update
        {
            var fullUrl = $"https://{host}:{port}{url}";
            using (var client = new HttpClient())
            {
                using(var request = new HttpRequestMessage(HttpMethod.Post, fullUrl)) //call httprequestmessage for post
                {
                    var json = JsonConvert.SerializeObject(obj); //serialize the object you passed in into json
                    using(var stringContent = new StringContent(json, Encoding.UTF8, "application/json")) //content of http request, pass in the json string, specify UTF8, specify that the string is json  
                    {
                        request.Content = stringContent; //sets the body of the post request to the json encoded data
                                                         //unlike the delete, post requires a body, which contains the data you want to send to the server

                        using(var response = await client
                            .SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                            .ConfigureAwait(false))
                        {
                            if(response.IsSuccessStatusCode)
                            {
                                return await response.Content.ReadAsStringAsync();
                            }
                            return "ERROR";
                        }
                    }
                }
            }
        }
    }
}
