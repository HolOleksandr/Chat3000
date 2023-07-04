using Chat.AzureFunc.TextFileTransform.FuncDbContext;
using Chat.AzureFunc.TextFileTransform.Services.Interfaces;
using Chat.BLL.Models.AzureFuncModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.AzureFunc.TextFileTransform.Services.Implementation
{
    public class TextInfoService : ITextInfoService
    {
        private readonly AppDbContext _appDbContext;
        public TextInfoService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddAsync(DocFileText textInfo)
        {
            await _appDbContext.DocFileText.AddAsync(textInfo);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
