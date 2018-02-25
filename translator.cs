using System;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json.Linq;

namespace translator
{
    class translator
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
            // Request parameters
            /*queryString["key"] = "trnsl.1.1.20180225T070421Z.f408b50a72b55847.adcaec1e142691d34c576dae138acc6145c9adf5";
            queryString["text"] = "凤爪";
            queryString["lang"] = "zh-en";*/
            string text = "凤爪";
            var uri = "https://translate.yandex.net/api/v1.5/tr.json/translate?key=trnsl.1.1.20180225T070421Z.f408b50a72b55847.adcaec1e142691d34c576dae138acc6145c9adf5&text="+text+"&lang=zh-en";
            var response = await client.GetAsync(uri);
            if (response.Content != null)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseString);              
            }
        }
    }
}
