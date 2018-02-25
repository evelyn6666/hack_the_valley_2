using System;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json.Linq;

namespace test2
{
    class Class1

    {
        static void Main()
        {
            MakeRequest();
            Console.WriteLine("Hit ENTER to exit...");
            Console.ReadLine();
        }

        static async void MakeRequest()
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "dd2d63038e6149cf923777ec190f1190");

            // Request parameters
            queryString["q"] = "pheonix claws";
            queryString["count"] = "15";
            queryString["offset"] = "0";
            queryString["mkt"] = "en-us";
            queryString["safeSearch"] = "Moderate";
            var uri = "https://api.cognitive.microsoft.com/bing/v7.0/search?" + queryString;

            var response = await client.GetAsync(uri);
            if (response.Content != null)
            {
                var responseString = await response.Content.ReadAsStringAsync();

                JObject mainObject = JObject.Parse(responseString);
                JObject webPages = (JObject) mainObject["webPages"];
                JArray data = (JArray)webPages["value"];
                for (int i = 0; i < data.Count; i++)
                {
                    string name = data[i]["name"].ToObject<string>();


                    if (name.Contains("Wikipedia") == true)
                    {
                        Console.WriteLine(data[i]["snippet"].ToObject<string>());
                    }
                }

            }
        }
    }
}