using System;
using System.IO;
using System.Threading.Tasks;
using Chat.BLL.Models.AzureFuncModels;
using Chat.BLL.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using DarkLoop.Azure.Functions.Authorize;

namespace Chat.AzureFunc.TextFileTransform
{
    public static class Main
    {
        [FunctionAuthorize]
        [FunctionName(nameof(TextFileConverter))]
        public static async Task<IActionResult> TextFileConverter(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")]  HttpRequest req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            var file = req.Form.Files["textFile"];
            var userDto = ServiceStack.Text.JsonSerializer.DeserializeFromString<UserDTO>(req.Form["UserDto"]);

            if (file != null & file.Length > 0 )
            {
                using var stream = file.OpenReadStream();
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                byte[] fileData = memoryStream.ToArray();

                var fileInfo = new FileInfoModel()
                {
                    UploaderId = userDto.Id,
                    FileName = file.FileName,
                    ContentType = file.ContentType,
                    Data = fileData
                };

                string instanceId = await starter.StartNewAsync("FileConvertorOrchestrator", null, fileInfo);
                log.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);
                return starter.CreateCheckStatusResponse(req, instanceId);
            }

            return new BadRequestObjectResult("File not found");
        }
    }
}