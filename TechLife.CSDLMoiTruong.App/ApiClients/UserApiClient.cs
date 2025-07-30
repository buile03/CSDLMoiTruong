using Newtonsoft.Json;
using System.Net;
using System.Text;
using TechLife.CSDLMoiTruong.App.ApiClients.Models;

namespace TechLife.CSDLMoiTruong.App.ApiClients
{
    public interface IUserApiClient
    {
        bool IsInRole(Guid userId, string role);
        bool IsInRoles(Guid userId, string[] role);
        Task<ApiViewModel<string>> GetToken(string userName, string passWord);
    }

    public class UserApiClient : BaseApiClient, IUserApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserApiClient> _logger;

        public UserApiClient(IHttpClientFactory httpClientFactory
            , IHttpContextAccessor httpContextAccessor
            , IConfiguration configuration, ILogger<UserApiClient> logger) : base(httpClientFactory, httpContextAccessor, configuration, logger)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<ApiViewModel<string>> GetToken(string userName, string passWord)
        {
            try
            {

                var client = _httpClientFactory.CreateClient();

                var request = new
                {
                    username = userName,
                    password = passWord
                };
                var json = JsonConvert.SerializeObject(request);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"http://syt.hue365.com/api/User/signin", httpContent);

                var result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<ApiViewModel<string>>(result);

                return new ApiViewModel<string>()
                {
                    IsSuccessed = false,
                    Message = "Xác thực không thành công!",
                    ResultObj = string.Empty,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return null;
            }
        }

        public bool IsInRole(Guid userId, string role)
        {
            return true;
        }

        public bool IsInRoles(Guid userId, string[] role)
        {
            return true;
        }
    }
}