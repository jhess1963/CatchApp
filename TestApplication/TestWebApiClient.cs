using CatchApp.Api.Models;
using CatchApp.Clubs;
using CatchApp.Geodata;
using CatchApp.Members;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TestApplication
{
    public class TestWebApiClient
    {
        HttpClient _client;
        string _url = @"api/services/catchapp/club/getclubs";

        // string _baseAddress = @"http://localhost:6634/";
        string _baseAddress = @"http://www.pamis.at/catchapp3/";

        public TestWebApiClient()
        {
        }

        //public async Task SearchPeople()
        //{
        //    HttpResponseMessage responseMessage = await _client.GetAsync(_url1);
        //    if (responseMessage.IsSuccessStatusCode)
        //    {
        //        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
        //        //var persons = JsonConvert.DeserializeObject<List<ClubSearchDto>>(responseData);
        //    }
        //}

        public async void SearchClubs()
        {
            string token = await Login();
            if (!string.IsNullOrEmpty(token))
            {
                string bearer = string.Format("Bearer {0}", token);


                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseAddress);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", bearer);

                    string url = "api/services/app/club/getclubs";

                    HttpResponseMessage resp = await client.PostAsync(url, null);
                    if (resp.IsSuccessStatusCode)
                    {
                        var respObject = await resp.Content.ReadAsAsync<WebApiContent>();
                        var output = JsonConvert.DeserializeObject<SearchClubsOutput>(respObject.result.ToString());
                        foreach (var club in output.Clubs)
                        {
                            Console.WriteLine("{0}", club.Name);
                        }
                    }
                    else
                        Console.WriteLine(string.Format("Error Code {0}: Message - {1}", resp.StatusCode, resp.ReasonPhrase));
                }
            }
        }

        public async Task InsertMemberImage()
        {
            string filename = @"C:\Users\jhess\Documents\Visual Studio 2015\Projects\CatchApp\CatchApp.EntityFramework\Data\Images\Hans.jpg";

            Image image = Image.FromFile(filename);
            var input = new UpdateMemberImageInput { UserName = "Hans" };
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Jpeg);
                input.Image = ms.ToArray();
            }


            string token = await Login();
            if (!string.IsNullOrEmpty(token))
            {
                string bearer = string.Format("Bearer {0}", token);


                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseAddress);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", bearer);

                    //string url = "api/services/app/club/searchclubs";
                    string url = "api/services/app/member/updatememberimage";

                    HttpResponseMessage resp = await client.PostAsJsonAsync<UpdateMemberImageInput>(url, input);
                    if (resp.IsSuccessStatusCode)
                    {
                        var respObject = await resp.Content.ReadAsStringAsync();
                        Console.WriteLine("Image inserted!");
                        Console.WriteLine(respObject);
                    }
                    else
                        Console.WriteLine(string.Format("Error Code {0}: Message - {1}", resp.StatusCode, resp.ReasonPhrase));
                }
            }
        }


        public void LoadClubImages()
        {
            string pfad = @"C:\Users\jhess\Documents\Visual Studio 2015\Projects\CatchApp\CatchApp.EntityFramework\Data\Images\Clubs";


        }

        public async Task InsertClubImage()
        {
            string filename = @"C:\Users\jhess\Documents\Manuel\Hessus App\Bar Italia\BarItalia.jpg";

            Image image = Image.FromFile(filename);
            var input = new UpdateClubImageInput { ClubId = 45 };
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Jpeg);
                input.Image = ms.ToArray();
            }


            string token = await Login();
            if (!string.IsNullOrEmpty(token))
            {
                string bearer = string.Format("Bearer {0}", token);


                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseAddress);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", bearer);

                    //string url = "api/services/app/club/searchclubs";
                    string url = "api/services/app/club/updateclubimage";

                    HttpResponseMessage resp = await client.PostAsJsonAsync<UpdateClubImageInput>(url, input);
                    if (resp.IsSuccessStatusCode)
                    {
                        var respObject = await resp.Content.ReadAsStringAsync();
                    }
                    else
                        Console.WriteLine(string.Format("Error Code {0}: Message - {1}", resp.StatusCode, resp.ReasonPhrase));
                }
            }
        }

        public async Task SearchClubsExt()
        {
            GeoPoint point1 = new GeoPoint(46.599550, 14.270328);

            var input = new SearchClubsInput {Latitude = point1.Latitude, Longitude = point1.Longitude, DistanceKm = 3.0, UserName = "hans" };

            string json = JsonConvert.SerializeObject(input);

            string token = await Login();
            if (!string.IsNullOrEmpty(token))
            {
                string bearer = string.Format("Bearer {0}", token);


                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseAddress);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", bearer);

                    string url = "api/services/app/club/searchclubs";



                    HttpResponseMessage resp = await client.PostAsJsonAsync<SearchClubsInput>(url, input);
                    if (resp.IsSuccessStatusCode)
                    {
                        var respObject = await resp.Content.ReadAsAsync<WebApiContent>();

                        var output = JsonConvert.DeserializeObject<SearchClubsOutput>(respObject.result.ToString());
                        foreach (var club in output.Clubs)
                        {
                            Console.WriteLine("{0}", club.Name);
                        }
                    }
                    else
                        Console.WriteLine(string.Format("Error Code {0}: Message - {1}", resp.StatusCode, resp.ReasonPhrase));
                }
            }
        }


        public async Task<string> Login()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = "api/Account/Authenticate";

                LoginModel login = new LoginModel
                {
                    TenancyName = "Default",
                    UsernameOrEmailAddress = "hans",
                    Password = "hans"
                };


                HttpResponseMessage resp = await client.PostAsJsonAsync<LoginModel>(url, login);
                if (resp.IsSuccessStatusCode)
                {
                    var output = await resp.Content.ReadAsAsync<WebApiContent>();
                    return output.result.ToString();
                }
                else
                    Console.WriteLine(string.Format("Error Code {0}: Message - {1}", resp.StatusCode, resp.ReasonPhrase));
            }
            return string.Empty;
        }

    }

    public class WebApiContent
    {
        public bool success { get; set; }
        public object result { get; set; }
        public object error { get; set; }
        public bool unAuthorizedRequest { get; set; }
    }

    public class InputSearchClubs
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double Distance { get; set; }
    }
}
