using MVCApp.Contracts;
using MVCApp.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MVCApp.Repository
{
    public class UserService : IUserService
    {
        private readonly IHttpClientFactory _clientFactory;
        private const string apiAddress = "api/v1/users";
        public UserService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<Tuple<string, string>> GenerateToken(LoginViewModel user)
        {
            var client = _clientFactory.CreateClient("WebAPI");
            using var response = await client.PostAsJsonAsync($"{apiAddress}/GenerateToken", user);
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var err = await response.Content.ReadAsAsync<string>();
                return await Task.FromResult(new Tuple<string, string>(err, string.Empty));
            }
            response.EnsureSuccessStatusCode();
            var token = await response.Content.ReadAsAsync<string>();
            return await Task.FromResult(new Tuple<string, string>(string.Empty, token));
        }

        public async Task<string> SignUpAsync(UserView user)
        {
            var client = _clientFactory.CreateClient("WebAPI");
            using var response = await client.PostAsJsonAsync(apiAddress, user);
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return await response.Content.ReadAsStringAsync();
            }
            response.EnsureSuccessStatusCode();
            return await Task.FromResult(string.Empty);
        }
    }
}
