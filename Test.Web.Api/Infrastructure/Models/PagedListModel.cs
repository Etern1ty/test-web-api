using System.Text.Json.Serialization;

namespace Test.Web.Api.Infrastructure.Models;

public class PagedListModel<T>(List<T> items, long itemsCount)
{
    [JsonPropertyName("items")]
    public List<T> Items { get; } = items;

    [JsonPropertyName("itemsCount")]
    public long ItemsCount { get; } = itemsCount;
}