using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Test.Web.Api.Models;

public class ObjectInsertModel
{
    [Required]
    [JsonPropertyName("code")]
    public int Code { get; set; }
    [Required]
    [JsonPropertyName("value")]
    public string Value { get; set; }
}