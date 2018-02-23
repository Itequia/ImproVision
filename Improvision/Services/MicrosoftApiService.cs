using Improvision.Models.InitialModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Improvision.Services
{
    
    public class MicrosoftApiService
    {
        const string uriBase = "https://westcentralus.api.cognitive.microsoft.com/vision/v1.0/recognizeText";
        const string subscriptionKey = "ca71a50ba3264dbb8ee09228326f83ec";


        public async Task<MicrosoftVisionAPIResult> GetImageJsonAsync(byte[] byteData)
        {
            HttpClient client = new HttpClient();

        //    // Request headers.
        //    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

        //    // Request parameters.
        //    string requestParameters = "language=unk&detectOrientation=true&handwriting=true";

        //    // Assemble the URI for the REST API Call.
        //    string uri = uriBase + "?" + requestParameters;

            // Request body. Posts a locally stored JPEG image.

            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                HttpResponseMessage response = await client.PostAsync(uri, content);
                string contentString = await response.Content.ReadAsStringAsync();
                MicrosoftVisionAPIResult result = JsonConvert.DeserializeObject<MicrosoftVisionAPIResult>(contentString);
                return result;
            }
        }
    }
}