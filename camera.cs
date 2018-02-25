using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace getText
{
    class Program {
        const string subscriptionKey = "8976b3597c8243c58962ea3ceb036377";
        const string uriBase = "https://westcentralus.api.cognitive.microsoft.com/vision/v1.0/ocr";
        //List<string> currencies = new List<string>(new string[] );
        public static readonly string[] currencies = { "元", "$" };
        static void Main(string[] args) {
            Console.WriteLine("Enter path to image:");
            string imagePath = Console.ReadLine();
            Task<List<List<string>>> words = TextRequest(imagePath);
            Console.WriteLine("\nPlease wait a moment for the results to appear. Then, press Enter to exit...\n");
            Console.ReadLine();
        }

        public static async Task<List<List<string>>> TextRequest(string imagePath){
            List<List<string>> allLines = new List<List<string>>();

            try {
                HttpClient client = new HttpClient();

                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

                string requestParameters = "language=unk&detectOrientation=true";
                // Assemble the URI for the REST API Call.
                string uri = uriBase + "?" + requestParameters;
                byte[] imageByteData = GetImageAsByteArray(imagePath);

                HttpResponseMessage response;

                using (ByteArrayContent content = new ByteArrayContent(imageByteData))
                {
                    // This example uses content type "application/octet-stream".
                    // The other content types you can use are "application/json" and "multipart/form-data".
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                    // Execute the REST API call.
                    response = await client.PostAsync(uri, content);

                    // Get the JSON response.
                    string contentString = await response.Content.ReadAsStringAsync();

                    // Display the JSON response.
                    Console.WriteLine("\nResponse:\n");
                   
                    //JObject j = JObject.Parse(contentString)["webpages"]["value"]["url"];

                    //string url = j.ToObject<string>();

                    JArray regions = (JArray)JObject.Parse(contentString)["regions"];
                    for (int i = 0; i < regions.Count; i++)
                    {
                        JArray line = (JArray)regions[i]["lines"];
                        for (int j = 0; j < line.Count; j++)
                        {   
                            List<string> wordsOnLine = new List<string>();
                            JArray words = (JArray)line[j]["words"];
                            for (int k = 0; k < words.Count; k++)
                            {
                                int n;
                                string s = words[k]["text"].ToObject<string>();
                                bool isNum = int.TryParse(s, out n);
                                bool isCurrency = false;

                                foreach(string currency in currencies){
                                    if (s == currency) isCurrency = true;
                                }
                                if(!isNum && !isCurrency) wordsOnLine.Add(s);
                            }
                            if(wordsOnLine.Count != 0) allLines.Add(wordsOnLine);
                        }
                    }
                }
                for (int i = 0; i < allLines.Count; i++){
                    Console.WriteLine(string.Join(" ", allLines[i].ToArray()));
                }

            } catch (Exception e){
                Console.WriteLine(e.Message);
            }
            return allLines;

        }

        static byte[] GetImageAsByteArray(string imageFilePath) {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }

    }
}
