using Chat.Blazor.Server.Helpers.Interfaces;
using Chat.Blazor.Server.Models;
using Chat.Blazor.Server.Models.DTO;
using Chat.Blazor.Server.Models.Paging;
using Chat.Blazor.Server.Models.Requests;
using Chat.Blazor.Server.Services.Interfaces;
using System.Text.Json;

namespace Chat.Blazor.Server.Services.Realization
{
    public class GroupService : IGroupService
    {
        private readonly string _baseUrl = "";
        private readonly IConfiguration _configuration;
        private readonly ICustomHttpClient _customHttpClient;

        public GroupService(ICustomHttpClient customHttpClient, IConfiguration configuration)
        {
            _customHttpClient = customHttpClient;
            _configuration = configuration;
            _baseUrl = _configuration["ApiUrls:ChatApi"];
            
        }

        public async Task<GroupDTO> GetGroupByIdAsync(int groupId)
        {
            using var response = await _customHttpClient
                .GetWithTokenAsync((_baseUrl + "api/group/" + groupId));
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            var requestResult = await ServiceStack.Text.JsonSerializer.DeserializeFromStreamAsync<GroupDTO>(stream);

            return requestResult;
        }

        public async Task<PagingResponse<GroupDTO>> GetAllUserGroupsWithSortAsync(string userEmail, string queryParams)
        {
            using var response = await _customHttpClient
                .GetWithTokenAsync((_baseUrl + "api/group/user/" + userEmail + queryParams));
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            var pagingResponse = await ServiceStack.Text.JsonSerializer.DeserializeFromStreamAsync<PagingResponse<GroupDTO>>(stream);

            return pagingResponse;
        }

        public async Task<RegistrationResult> CreateNewGroup(CreateGroupRequest createGroupRequest)
        {
            var result = await _customHttpClient.PostWithTokenAsync(_baseUrl + "api/group/new", createGroupRequest);

            if (result.IsSuccessStatusCode)
                return new RegistrationResult { Success = true, Errors = null };
            var registrationResult = System.Text.Json.JsonSerializer.Deserialize<Error>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return new RegistrationResult { Success = false, Errors = new List<string> { registrationResult.Message } };
        }
    }
}
