using AutoMapper;
using Chat.BLL.DTO;
using Chat.BLL.Services.Interfaces;
using Chat.DAL.Entities;

namespace Chat.BLL.Automapper.Resolvers
{
    public class AvatarResolver : IValueResolver<User, UserDTO, string?>
    {
        private readonly IBlobManager _blobManager;

        public AvatarResolver(IBlobManager blobManager)
        {
            _blobManager = blobManager;
        }

        public string? Resolve(User source, UserDTO destination, string? destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.Avatar))
            {
                return null;
            }
            return _blobManager.GetBlobSasUri(source.Avatar, 1);
        }
    }
}
