using Moq;
using RandomQuotes;
using RandomQuotes.Models;

namespace Test;

public class QuoteServiceTests
{
    private static Quote CreateQuote(Guid id, string text = "text")
        => new Quote()
        {
            Id = id,
            Text = text,
            Author = "Author",
            Origin = "Origin",
            Categories = ["Cat"]
        };

    [Fact]
    public async Task TryGetRandom_ReturnsNull_WhenNoQuotes()
    {
        // arrange
        var mockRepo = new Mock<IQuoteRepository>();
        mockRepo.Setup(r => r.GetAllAsync(CancellationToken.None))
            .ReturnsAsync([]);

        var service = new QuoteService(mockRepo.Object);
        var userId = Guid.NewGuid();

        // act
        var result = await service.GetRandomAsync(userId);

        // assert
        Assert.Null(result);
        mockRepo.Verify(r => r.GetAllAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task TryGetRandom_EventuallyReturnsNull_WhenAllSeen()
    {
        // arrange
        var quotes = new[]
        {
            CreateQuote(Guid.NewGuid(), "Q1"),
            CreateQuote(Guid.NewGuid(), "Q2"),
            CreateQuote(Guid.NewGuid(), "Q3")
        };

        var mockRepo = new Mock<IQuoteRepository>();
        mockRepo.Setup(r => r.GetAllAsync(CancellationToken.None))
            .ReturnsAsync(quotes);

        var service = new QuoteService(mockRepo.Object);
        var userId = Guid.NewGuid();

        // act
        var results = new Quote?[quotes.Length + 1];
        for (int i = 0; i < results.Length; i++)
        {
            results[i] = await service.GetRandomAsync(userId);
        }

        // assert
        Assert.Equal(3, results.Take(3).Count(q => q is not null)); // 3 quotes
        Assert.Null(results.Last()); // the extra call should be null
    }

    [Fact]
    public void Reset_MakesQuotesAvailableAgain()
    {
        // arrange
        var quoteId = Guid.NewGuid();
        var quotes = new[]
        {
            CreateQuote(quoteId, "Q1")
        };

        var mockRepo = new Mock<IQuoteRepository>();
        mockRepo.Setup(r => r.GetAllAsync(CancellationToken.None))
            .ReturnsAsync(quotes);

        var service = new QuoteService(mockRepo.Object);
        var userId = Guid.NewGuid();

        // act
        var first = service.GetRandomAsync(userId);
        var second = service.GetRandomAsync(userId); // should be null
        service.Reset(userId);
        var third = service.GetRandomAsync(userId); // should be available again

        // assert
        Assert.NotNull(first);
        Assert.Null(second);
        Assert.NotNull(third);
    }
}