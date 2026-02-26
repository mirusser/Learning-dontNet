using System.Text.Json.Serialization;

namespace RandomQuotesApi.Models.DTOs;

public class QuotableRandomResponse
{
    [JsonPropertyName("_id")]
    public string Id { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }

    [JsonPropertyName("author")]
    public string Author { get; set; }

    [JsonPropertyName("tags")]
    public string[] Tags { get; set; }

    [JsonPropertyName("authorSlug")]
    public string AuthorSlug { get; set; }

    [JsonPropertyName("length")]
    public int Length { get; set; }

    [JsonPropertyName("dateAdded")]
    public string DateAdded { get; set; }

    [JsonPropertyName("dateModified")]
    public string DateModified { get; set; }
}