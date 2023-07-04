using Chat.Blazor.WebAssembly.Helpers.Interfaces;
using Chat.Blazor.WebAssembly.Models;
using Chat.Blazor.WebAssembly.Models.DTO;
using Chat.Blazor.WebAssembly.Models.Responses;
using Chat.Blazor.WebAssembly.Services.Interfaces;
using System.Text.Json;

namespace Chat.Blazor.WebAssembly.Services.Implementation
{
    public class ChatMessageService : IChatMessageService
    {
        private readonly string _baseUrl = "";
        private readonly IConfiguration _configuration;
        private readonly ICustomHttpClient _customHttpClient;

        public ChatMessageService(ICustomHttpClient customHttpClient, IConfiguration configuration)
        {
            _customHttpClient = customHttpClient;
            _configuration = configuration;
            _baseUrl = _configuration.GetSection("ChatApi").Value;
        }

        public async Task<IEnumerable<MessageDTO>> GetAllMessagesInGroupAsync(int groupId)
        {
            using var response = await _customHttpClient
                .GetWithTokenAsync((_baseUrl + "api/message/" + groupId));
            var stream = await response.Content.ReadAsStreamAsync();
            var pagingResponse = await ServiceStack.Text.JsonSerializer.DeserializeFromStreamAsync<IEnumerable<MessageDTO>>(stream);

            return pagingResponse;
        }

        public async Task<RequestResult> SendMessageAsync(MessageDTO newMessage)
        {
            var result = await _customHttpClient.PostWithTokenAsync(_baseUrl + "api/message/add", newMessage);

            if (result.IsSuccessStatusCode)
                return new RequestResult { Success = true, Errors = null };
            var requestResult = JsonSerializer.Deserialize<Error>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return new RequestResult { Success = false, Errors = new List<string> { requestResult.Message } };
        }
    }
}