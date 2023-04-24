using Chat.Blazor.Server.Data;
using Chat.Blazor.Server.Helpers.Interfaces;
using Chat.Blazor.Server.Models;
using Chat.Blazor.Server.Models.Paging;
using Chat.Blazor.Server.Services.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using ServiceStack;
using ServiceStack.Text;
using System.IO;
using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;


namespace Chat.Blazor.Server.Services.Realization
{
    public class UserService : IUserService
    {
        private readonly string _baseUrl = "";
        private readonly IConfiguration _configuration;
        private readonly ICustomHttpClient _customHttpClient;
        private readonly HttpClient _httpClient;

        public UserService(ICustomHttpClient customHttpClient, IConfiguration configuration, HttpClient httpClient)
        {
            _customHttpClient = customHttpClient;
            _configuration = configuration;
            _baseUrl = _configuration["ApiUrls:ChatApi"];
            _httpClient = httpClient;
        }

        public async Task<PagingResponse<UserDTO>> GetAllUsersWithSortAsync(string queryParams)
        {
            using var response = await _customHttpClient
                .GetWithTokenAsync((_baseUrl + "api/user/all" + queryParams));

            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            var pagingResponse = await ServiceStack.Text.JsonSerializer.DeserializeFromStreamAsync<PagingResponse<UserDTO>>(stream);
           
            return pagingResponse;
        }

        public async Task<UserDTO> GetUserByIdAsync(string userId)
        {
            using var response = await _customHttpClient
                .GetWithTokenAsync((_baseUrl + "api/user/id/" + userId));

            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            var userDto = await ServiceStack.Text.JsonSerializer.DeserializeFromStreamAsync<UserDTO>(stream);

            return userDto;
        }


        public async Task<RegistrationResult> UpdateUserAsync(UserDTO updateUserModel)
        {

            var result = await _customHttpClient.PostWithTokenAsync(_baseUrl + "api/user/update", updateUserModel);

            if (result.IsSuccessStatusCode)
                return new RegistrationResult { Success = true, Errors = null };
            var registrationResult = System.Text.Json.JsonSerializer.Deserialize<Error>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return new RegistrationResult { Success = false, Errors = new List<string> { registrationResult.Message } };
        }

    }
}
