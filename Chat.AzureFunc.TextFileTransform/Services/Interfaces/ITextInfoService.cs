using Chat.BLL.Models.AzureFuncModels;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.AzureFunc.TextFileTransform.Services.Interfaces
{
    public interface ITextInfoService
    {
        Task AddAsync(DocFileText textInfo);
    }
}
