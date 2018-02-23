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
            //HttpClient client = new HttpClient();

            //// Request headers.
            //client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            //// Request parameters.
            //string requestParameters = "language=unk&detectOrientation=true&handwriting=true";

            //// Assemble the URI for the REST API Call.
            //string uri = uriBase + "?" + requestParameters;

            //// Request body. Posts a locally stored JPEG image.

            //using (ByteArrayContent content = new ByteArrayContent(byteData))
            //{
            //    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            //    HttpResponseMessage response = await client.PostAsync(uri, content);
            //    string contentString = await response.Content.ReadAsStringAsync();
            //    MicrosoftVisionAPIResult result = JsonConvert.DeserializeObject<MicrosoftVisionAPIResult>(contentString);
            //    return result;
            //}

            HttpClient client = new HttpClient();

            // Request headers.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            // Request parameter. Set "handwriting" to false for printed text.
            string requestParameters = "handwriting=true";

            // Assemble the URI for the REST API Call.
            string uri = uriBase + "?" + requestParameters;

            HttpResponseMessage response = null;

            // This operation requrires two REST API calls. One to submit the image for processing,
            // the other to retrieve the text found in the image. This value stores the REST API
            // location to call to retrieve the text.
            string operationLocation = null;

            // Request body. Posts a locally stored JPEG image.
            //byte[] byteData = GetImageAsByteArray(imageFilePath);
            ByteArrayContent content = new ByteArrayContent(byteData);

            // This example uses content type "application/octet-stream".
            // You can also use "application/json" and specify an image URL.
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            // The first REST call starts the async process to analyze the written text in the image.
            response = await client.PostAsync(uri, content);

            // The response contains the URI to retrieve the result of the process.
            if (response.IsSuccessStatusCode)
                operationLocation = response.Headers.GetValues("Operation-Location").FirstOrDefault();
            else
            {
                // Display the JSON error data.
                //Console.WriteLine("\nError:\n");
                //Console.WriteLine(JsonPrettyPrint(await response.Content.ReadAsStringAsync()));
                //return;
            }

            // The second REST call retrieves the text written in the image.
            //
            // Note: The response may not be immediately available. Handwriting recognition is an
            // async operation that can take a variable amount of time depending on the length
            // of the handwritten text. You may need to wait or retry this operation.
            //
            // This example checks once per second for ten seconds.
            string contentString;
            int i = 0;
            do
            {
                System.Threading.Thread.Sleep(1000);
                response = await client.GetAsync(operationLocation);
                contentString = await response.Content.ReadAsStringAsync();
                ++i;
            }
            while (i < 10 && contentString.IndexOf("\"status\":\"Succeeded\"") == -1);

            if (i == 10 && contentString.IndexOf("\"status\":\"Succeeded\"") == -1)
            {
                //Console.WriteLine("\nTimeout error.\n");
                //return;
            }
            MicrosoftVisionAPIResult result = JsonConvert.DeserializeObject<MicrosoftVisionAPIResult>(contentString);
            return result;
        }
    }
}