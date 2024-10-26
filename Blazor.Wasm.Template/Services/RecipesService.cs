using SharedLibraryTemplate.Models;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;

namespace Blazor.Wasm.Template.Services
{
    public class RecipesService : IRecipesService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string API_KEY= "2bd4a3d07a2c49cba0bf22fbdf6adedb";
        public RecipesService(IHttpClientFactory httpClientFactory)
        {
                _httpClientFactory=httpClientFactory;
        }
        public async Task<RecipeList> Search(string searchText)
        {
            return await GetFromApiAsync<RecipeList>($"/recipes/complexSearch?apiKey={API_KEY}&query={searchText}");
        }

        public async Task<RecipeDetails> GetRecipeDetails(int id)
        {
            return await GetFromApiAsync<RecipeDetails>($"/recipes/{id}/information?apiKey={API_KEY}");
        }


        #region Helpers

        private async Task<T> GetFromApiAsync<T>(string url)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("Spoonacular");
                var response = await httpClient.GetAsync(url);
                string responseContent = await response.Content.ReadAsStringAsync();
                var result= JsonSerializer.Deserialize<T>(responseContent);
                if (result == null) throw new Exception("Deserialization returned null");
                return result;
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine("Not found");
                throw new Exception("Resource not found",ex);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                Console.WriteLine("Internal server error occured");
                throw new Exception("Server error, please try again later",ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                throw new Exception("An unexpected error occured, try again later", ex);
            }

        }
        #endregion
    }
}
