namespace RandomQuotesApi.Data;

public class QuoteSeedDto
{
    public string Text { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Origin { get; set; } = string.Empty;
    public string[] Categories { get; set; } = [];
}