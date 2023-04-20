using Chat.Blazor.Server.Helpers.Interfaces;
using Chat.Blazor.Server.Models;
using Chat.Blazor.Server.Models.Paging;
using Chat.Blazor.Server.Services.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using ServiceStack;
using ServiceStack.Text;
using System.IO;

namespace Chat.Blazor.Server.Services.Realization
{
    public class UserService : IUserService
    {
        private readonly string _baseUrl = "";
        private readonly IConfiguration _configuration;
        private readonly ICustomHttpClient _customHttpClient;


        public UserService(ICustomHttpClient customHttpClient, IConfiguration configuration)
        {
            _customHttpClient = customHttpClient;
            _configuration = configuration;
            _baseUrl = _configuration["ApiUrls:ChatApi"];
        }

        public async Task<PagingResponse<UserDTO>> GetAllUsersWithSortAsync(string queryParams)
        {

            using var response = await _customHttpClient
                .GetWithTokenAsync((_baseUrl + "api/user/all" + queryParams));

            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            var pagingResponse = await JsonSerializer.DeserializeFromStreamAsync<PagingResponse<UserDTO>>(stream);
           
            return pagingResponse;


        }

    }
}
