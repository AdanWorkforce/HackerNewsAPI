using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using HackerNewsAPI.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace HackerNewsAPI.Services
{
    public class HackerNewsService : IHackerNewsService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HackerNewsService> _logger;
        private readonly MemoryCache _cache;
        private readonly SemaphoreSlim _semaphore;

        private const string BestStoriesUrl = "https://hacker-news.firebaseio.com/v0/beststories.json";
        private const string StoryDetailUrl = "https://hacker-news.firebaseio.com/v0/item/{0}.json";
        private const int CacheExpirationMinutes = 5;
        private const int MaxConcurrentRequests = 10;

        public HackerNewsService(HttpClient httpClient, ILogger<HackerNewsService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _cache = new MemoryCache(new MemoryCacheOptions());
            _semaphore = new SemaphoreSlim(MaxConcurrentRequests, MaxConcurrentRequests);
        }

        public async Task<IEnumerable<StoryResponse>> GetBestStoriesAsync(int n)
        {
            if (n <= 0)
                return Enumerable.Empty<StoryResponse>();

            try
            {
                // Get Ids
                var storyIds = await GetBestStoryIdsAsync();

                // Get Details
                var topStoryIds = storyIds.Take(n);
                var stories = await GetStoriesDetailsAsync(topStoryIds);

                // Orderby Score
                return stories
                    .OrderByDescending(s => s.Score)
                    .Select(MapToStoryResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving best stories");
                throw;
            }
        }

        private async Task<int[]> GetBestStoryIdsAsync()
        {
            var cacheKey = "best_stories_ids";

            if (_cache.TryGetValue(cacheKey, out int[] cachedIds))
                return cachedIds;

            var response = await _httpClient.GetStringAsync(BestStoriesUrl);
            var storyIds = JsonSerializer.Deserialize<int[]>(response);

            // Cache Id, 5mins
            _cache.Set(cacheKey, storyIds, TimeSpan.FromMinutes(CacheExpirationMinutes));

            return storyIds;
        }

        private async Task<List<Story>> GetStoriesDetailsAsync(IEnumerable<int> storyIds)
        {
            var tasks = storyIds.Select(GetStoryDetailAsync);
            var stories = await Task.WhenAll(tasks);
            return stories.Where(s => s != null).ToList();
        }

        private async Task<Story> GetStoryDetailAsync(int storyId)
        {
            var cacheKey = $"story_{storyId}";

            if (_cache.TryGetValue(cacheKey, out Story cachedStory))
                return cachedStory;

            await _semaphore.WaitAsync();
            try
            {
                // Double verification cache
                if (_cache.TryGetValue(cacheKey, out cachedStory))
                    return cachedStory;

                var url = string.Format(StoryDetailUrl, storyId);
                var response = await _httpClient.GetStringAsync(url);
                var story = JsonSerializer.Deserialize<Story>(response);

                if (story != null && !string.IsNullOrEmpty(story.Title))
                {
                    _cache.Set(cacheKey, story, TimeSpan.FromMinutes(CacheExpirationMinutes));
                }

                return story;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error retrieving story {StoryId}", storyId);
                return null;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private StoryResponse MapToStoryResponse(Story story)
        {
            return new StoryResponse
            {
                Title = story.Title,
                Uri = story.Url,
                PostedBy = story.By,
                Time = DateTimeOffset.FromUnixTimeSeconds(story.Time).DateTime,
                Score = story.Score,
                CommentCount = story.Descendants
            };
        }
    }
}