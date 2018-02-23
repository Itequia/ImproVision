using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Improvision.Models;
using System.Net.Http.Headers;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using System.IO;
using Improvision.Services;
using Improvision.Models.InitialModels;

namespace Improvision.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public async Task<IActionResult> GetUploadedFiles()
        {
            var files = HttpContext.Request.Form.Files;
            MicrosoftVisionAPIResult result = new MicrosoftVisionAPIResult();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName.Trim('"');
                    using (var reader = new StreamReader(formFile.OpenReadStream()))
                    {

                        string contentAsString = reader.ReadToEnd();
                        byte[] bytes = new byte[contentAsString.Length * sizeof(char)];

                        MicrosoftApiService microsoftApiService = new MicrosoftApiService();
                        result = await microsoftApiService.GetImageJsonAsync(bytes);
                    }
                }
            }
            return View(result.recognitionResult.lines.SelectMany(l => l.words));
        }

        public IActionResult Blackboard()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        //[HttpPost("UploadFiles")]
        //public async Task<IActionResult> Post(List<IFormFile> files)
        //{
        //    long size = files.Sum(f => f.Length);

        //    // full path to file in temp location
        //    var filePath = Path.GetTempFileName();

        //    foreach (var formFile in files)
        //    {
        //        if (formFile.Length > 0)
        //        {

        //            if (formFile.Length > 0)
        //            {

        //                var fileName = ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName.Trim('"');
        //                using (var reader = new StreamReader(formFile.OpenReadStream()))
        //                {

        //                    string contentAsString = reader.ReadToEnd();
        //                    byte[] bytes = new byte[contentAsString.Length * sizeof(char)];

        //                    MicrosoftApiService microsoftApiService = new MicrosoftApiService();
        //                    await microsoftApiService.GetImageJsonAsync(bytes);


        //                }
        //            }
        //        }
        //    }

        //    return Ok(new { count = files.Count, size, filePath });
        //}

    }

}
