using AutoMapper;
using Chat.BLL.DTO;
using Chat.BLL.Services.Interfaces;
using Chat.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.BLL.Automapper.Resolvers
{
    public class PdfContractResolver : IValueResolver<PdfContract, PdfContractDTO, string?>
    {
        private readonly IBlobManager _blobManager;

        public PdfContractResolver(IBlobManager blobManager)
        {
            _blobManager = blobManager;
        }

        public string? Resolve(PdfContract source, PdfContractDTO destination, string? destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.FileUrl))
            {
                return null;
            }
            return _blobManager.GetBlobSasUri(source.FileUrl, 24);
        }
    }
}
