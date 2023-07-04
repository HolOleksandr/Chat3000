using Chat.BLL.DTO;
using Chat.BLL.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat.BLL.Models.Requests
{
    public class UserUpdateRequest
    {
        [ModelBinder(BinderType = typeof(FormDataJsonBinder))]
        public UserDTO UserDTO { get; set; }
        public IFormFile? File { get; set; }
    }
}
