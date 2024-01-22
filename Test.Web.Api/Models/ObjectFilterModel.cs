using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Test.Web.Api.Enums;
using Test.Web.Api.Infrastructure;
using Test.Web.Api.Interfaces;

namespace Test.Web.Api.Models;

public class ObjectFilterModel: JsonModel, IPagingModel, ISearchModel
{
    [Required]
    [JsonPropertyName("take")]
    public int Take { get; set; }
    [Required]
    [JsonPropertyName("skip")]
    public int Skip { get; set; }
    [JsonPropertyName("keyword")]
    public string? Keyword { get; set; }
}