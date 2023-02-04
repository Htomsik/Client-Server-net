using System.Net;
using System.Net.Http.Json;
using Interfaces.Entities;
using Interfaces.Repositories;

namespace HTTPClients.Repositories;

public class HttpRepository<T> : IRepository<T> where T : IEntity
{
    #region Fields
    protected readonly HttpClient Client;
    #endregion
    
    #region Records/Classes
    private class ItemPageItem : IPageItem<T>
    {
        public IEnumerable<T> Items { get; init; }
        public int TotalCount { get; init; }
        public int PageIndex { get; init;}
        public int PageSize { get; init; }
        
        public int TotalPagesCount => (int)Math.Ceiling((double)TotalCount / PageSize);
    }

  

    #endregion
    
    #region Constructors
    public HttpRepository(HttpClient client) => Client = client;
    #endregion
    
    #region Methods
    #region Page interactions
    public async Task<IPageItem<T>> GetPage(int pageIndex, int pageSize, CancellationToken cancel = default)
    {
        var response = await Client.GetAsync($"page[{pageIndex}:{pageSize}]", cancel).ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return new ItemPageItem
            {
                Items = Enumerable.Empty<T>(),
                TotalCount = 0,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }

        return await response
            .EnsureSuccessStatusCode()
            .Content
            .ReadFromJsonAsync<ItemPageItem>(cancellationToken: cancel)
            .ConfigureAwait(false)!;
    }
    #endregion
    
    #region Collections interactions
    public async Task<IEnumerable<T>> GetMany(CancellationToken cancel = default) =>
        await Client.GetFromJsonAsync<IEnumerable<T>>("items", cancel).ConfigureAwait(false)!;
    
    public async Task<IEnumerable<T>> GetMany(int skip, int count, CancellationToken cancel = default) =>
        await Client.GetFromJsonAsync<IEnumerable<T>>($"items[{skip}:{count}]", cancel).ConfigureAwait(false)!;
    #endregion

    #region Item interactions

    public async Task<T?> Get(int id, CancellationToken cancel = default) =>
        await Client.GetFromJsonAsync<T>($"{id}",cancel).ConfigureAwait(false);
    
    public async Task<bool> Add(T item, CancellationToken cancel = default)
    {
        var response = await Client.PostAsJsonAsync("", item, cancel).ConfigureAwait(false);

        return response.StatusCode != HttpStatusCode.BadRequest && response.IsSuccessStatusCode;
    }
    
    public async Task<bool> Update( T item, CancellationToken cancel = default)
    {
        var response = await Client.PutAsJsonAsync("", item, cancel).ConfigureAwait(false);

        return response.StatusCode != HttpStatusCode.NotFound && response.IsSuccessStatusCode;
    }

    public async Task<bool> Delete(T item, CancellationToken cancel = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, "")
        {
            Content = JsonContent.Create(item)
        };

        var response = await Client.SendAsync(request, cancel).ConfigureAwait(false);
        
        return response.StatusCode != HttpStatusCode.NotFound && response.IsSuccessStatusCode;
    }

    public async Task<bool> Delete(int id, CancellationToken cancel = default)
    {
        var response = await Client.DeleteAsync($"{id}", cancel).ConfigureAwait(false);

        return response.StatusCode != HttpStatusCode.NotFound && response.IsSuccessStatusCode;
    }
    #endregion

    #region Extensions
    public async Task<int> Count(CancellationToken cancel = default) =>
        await Client.GetFromJsonAsync<int>("count", cancel).ConfigureAwait(false);
    
    public async Task<bool> Exist(int id, CancellationToken cancel = default)
    {
        var response = await Client.GetAsync($"exist/id/{id}",cancel).ConfigureAwait(false);

        return response.StatusCode != HttpStatusCode.NotFound && response.IsSuccessStatusCode;
    }

    public async Task<bool> Exist(T item, CancellationToken cancel = default)
    {
        var response = await Client.PostAsJsonAsync("exist", item, cancel).ConfigureAwait(false);

        return response.StatusCode != HttpStatusCode.NotFound && response.IsSuccessStatusCode;
    }
    #endregion
    #endregion
}