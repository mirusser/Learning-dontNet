using Polly;
using Polly.Registry;
using RandomQuotesApi.Models.DTOs;

namespace RandomQuotesApi.Clients;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? Error { get; }

    private Result(bool isSuccess, T? value, string? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public static Result<T> Success(T value) => new(true, value, null);

    public static Result<T> Failure(string error) => new(false, default, error);
}

public interface IQuotableClient
{
    Task<Result<QuotableRandomResponse>> GetRandomQuoteAsync(CancellationToken cancellationToken = default);
}

public class QuotableClient : IQuotableClient
{
    private readonly HttpClient client;
    private readonly ResiliencePipeline pipeline;
    private static readonly Uri BaseUri = new("http://api.quotable.io/");

    public QuotableClient(
        IHttpClientFactory clientFactory,
        ResiliencePipelineProvider<string> pipelineProvider)
    {
        client = clientFactory.CreateClient();

        // Configure the client here (your requirement)
        client.BaseAddress = BaseUri;
        client.DefaultRequestHeaders.Accept.ParseAdd("application/json");

        pipeline = pipelineProvider.GetPipeline("resilience-pipeline");
    }

    public async Task<Result<QuotableRandomResponse>> GetRandomQuoteAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // Execute the HTTP call through the Polly v8 pipeline
            HttpResponseMessage response = await pipeline.ExecuteAsync(
                async token => await client.GetAsync("random", token),
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return Result<QuotableRandomResponse>.Failure(
                    $"Quotable API returned {(int)response.StatusCode} ({response.ReasonPhrase})");
            }

            var quote =
                await response.Content.ReadFromJsonAsync<QuotableRandomResponse>(cancellationToken: cancellationToken);

            if (quote is null)
            {
                return Result<QuotableRandomResponse>.Failure("Failed to deserialize quote from Quotable API.");
            }

            return Result<QuotableRandomResponse>.Success(quote);
        }
        catch (Exception ex)
        {
            // You can add logging here
            return Result<QuotableRandomResponse>.Failure($"Exception while calling Quotable API: {ex.Message}");
        }
    }
}