using SharedLibraryTemplate.Models;
namespace Blazor.Wasm.Template.Services
{
    public interface IRecipesService
    {
        Task<RecipeList> Search(string searchText);
        Task<RecipeDetails> GetRecipeDetails(int id);

    }
}
