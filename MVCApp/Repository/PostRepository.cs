using MVCApp.Contracts;
using MVCApp.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MVCApp.Repository
{
    public class PostRepository : IPostRepository
    {
        private const string apiAddress = "api/v1/posts";
        private readonly IHttpClientFactory _clientFactory;

        public PostRepository(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<HttpResponseMessage> AddPost(Post post, string token)
        {
            var client = _clientFactory.CreateClient("WebAPI");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetJWTToken(token));
            using (var response = await client.PostAsJsonAsync(apiAddress, post)) 
            {
                response.EnsureSuccessStatusCode();
                return response;
            }
        }

        public async Task<List<PostViewModel>> GetAllAsync()
        {
            var client = _clientFactory.CreateClient("WebAPI");
            using (var response = await client.GetAsync(apiAddress))
            {
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsAsync<List<PostViewModel>>();
            }
        }

        public async Task<HttpResponseMessage> DeletePost(int Id, string token)
        {
            var client = _clientFactory.CreateClient("WebAPI");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetJWTToken(token));
            using (var response = await client.DeleteAsync($"{apiAddress}/{Id}"))
            {
                response.EnsureSuccessStatusCode();
                return response;
            }
        }

        public async Task<HttpResponseMessage> EditPost(Post post, string token)
        {
            var client = _clientFactory.CreateClient("WebAPI");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetJWTToken(token));
            using (var response = await client.PutAsJsonAsync($"{apiAddress}/{post.Id}", post))
            {
                response.EnsureSuccessStatusCode();
                return response;
            }
        }

        public async Task<Post> GetPost(int Id)
        {
            var client = _clientFactory.CreateClient("WebAPI");
            using (var response = await client.GetAsync($"{apiAddress}/{Id}"))
            {
                response.EnsureSuccessStatusCode();
                var post =  await response.Content.ReadAsAsync<Post>();
                return post;
            }
        }

        private static string GetJWTToken(string authToken)
        {
            if(string.IsNullOrEmpty(authToken))
            {
                return string.Empty;
            }
            var split = authToken.Split(' ');
            if(split.Length < 2)
            {
                return string.Empty;
            }
            return split[1];
        }
    }
}
