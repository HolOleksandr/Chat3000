using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs;
using System;
using System.Threading.Tasks;
using Chat.BLL.Models.Responses;
using Chat.BLL.Models.AzureFuncModels;
using Chat.BLL.Exceptions;
using Microsoft.Extensions.Logging;


namespace Chat.AzureFunc.TextFileTransform
{
    public static class OrchestratorFunctions
    {
        [FunctionName(nameof(FileConvertorOrchestrator))]
        public static async Task<FileConvertorResponse> FileConvertorOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context, ILogger log)
        {
            var retryOptions = new RetryOptions(
                firstRetryInterval: TimeSpan.FromSeconds(5),
                maxNumberOfAttempts: 3);

            try
            {
                var fileInfo = context.GetInput<FileInfoModel>();
                await context.CallActivityAsync("CheckFileExtention", fileInfo);
                var fileUrl = await context.CallActivityWithRetryAsync<string>("UploadFile", retryOptions, fileInfo);
                var fileText = await context.CallActivityWithRetryAsync<string>("GetFileText", retryOptions, fileUrl);

                var textInfo = new DocFileText { Text = fileText , FileName = fileInfo.FileName, UploaderId = fileInfo.UploaderId};
                await context.CallActivityWithRetryAsync("WriteTextToDb", retryOptions, textInfo);
                return new FileConvertorResponse() { Success = true, Text = fileText };
            }
            catch (AzureFuncException ex)
            {
                var m = ex.Message;
                log.LogError("Error in Durable");
                return new FileConvertorResponse() { Success = false, Text = "Mesage from inner Exeption" };
            }
            catch (Exception)
            {
                return new FileConvertorResponse() { Success = false, Text = "Invalid operation" };
            }
            
        }
    }
}
