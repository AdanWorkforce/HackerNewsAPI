using HackerNewsAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HackerNewsAPI.Services
{
    public interface IHackerNewsService
    {
        Task<IEnumerable<StoryResponse>> GetBestStoriesAsync(int n);
    }
}