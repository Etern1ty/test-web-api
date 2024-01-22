using System.Text.Json.Serialization;

namespace Test.Web.Api.Models;

public class ObjectItemModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("code")]
    public int Code { get; set; }
    [JsonPropertyName("value")]
    public string Value { get; set; }
}