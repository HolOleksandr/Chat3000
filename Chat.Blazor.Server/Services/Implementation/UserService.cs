﻿using Chat.Blazor.Server.Helpers.Interfaces;
using Chat.Blazor.Server.Models;
using Chat.Blazor.Server.Models.Paging;
using Chat.Blazor.Server.Services.Interfaces;
using Chat.Blazor.Server.Models.DTO;
using Microsoft.AspNetCore.Components;

namespace Chat.Blazor.Server.Services.Implementation
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

            var stream = await response.Content.ReadAsStreamAsync();
            var pagingResponse = await ServiceStack.Text.JsonSerializer.DeserializeFromStreamAsync<PagingResponse<UserDTO>>(stream);
            return pagingResponse;
        }

        public async Task<UserDTO> GetUserByIdAsync(string userId)
        {
            using var response = await _customHttpClient
                .GetWithTokenAsync((_baseUrl + "api/user/id/" + userId));
            var stream = await response.Content.ReadAsStreamAsync();
            var userDto = await ServiceStack.Text.JsonSerializer.DeserializeFromStreamAsync<UserDTO>(stream);
            return userDto;
        }


        public async Task<RegistrationResult> UpdateUserAsync(UserDTO updateUserModel)
        {

            var result = await _customHttpClient.PostWithTokenAsync(_baseUrl + "api/user/update", updateUserModel);

            if (result.IsSuccessStatusCode)
                return new RegistrationResult { Success = true, Errors = null };
            var stream = await result.Content.ReadAsStreamAsync();
            var registrationResult = await ServiceStack.Text.JsonSerializer.DeserializeFromStreamAsync<Error>(stream);

            return new RegistrationResult { Success = false, Errors = new List<string>() { registrationResult.Message } };
        }

        public async Task<IEnumerable<string>> GetUsersEmailsExcepMaker(string makerEmail)
        {
            using var response = await _customHttpClient
                .GetWithTokenAsync((_baseUrl + "api/user/all/emails/" + makerEmail));

            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            var emails = await ServiceStack.Text.JsonSerializer.DeserializeFromStreamAsync<IEnumerable<string>>(stream);

            return emails;
        }

        public async Task<IEnumerable<UserShortInfoDTO>> GetUsersShortInfoExceptMaker(string makerEmail)
        {
            using var response = await _customHttpClient
                .GetWithTokenAsync((_baseUrl + "api/user/allinfo/except/" + makerEmail));

            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            var userDto = await ServiceStack.Text.JsonSerializer.DeserializeFromStreamAsync<IEnumerable<UserShortInfoDTO>>(stream);

            return userDto;
        }

    }
}