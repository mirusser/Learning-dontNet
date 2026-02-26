using Moq;
using RandomQuotesApi.Models;
using RandomQuotesApi.Repos;
using RandomQuotesApi.Services;

namespace Tests;

public class QuoteServiceTests
{
    private static List<Quote> CreateSampleQuotes()
    {
        return
        [
            new Quote { Id = Guid.NewGuid(), Text = "Q1", Author = "A1", Origin = "O1", Categories = ["c1"] },
            new Quote { Id = Guid.NewGuid(), Text = "Q2", Author = "A2", Origin = "O2", Categories = ["c2"] },
            new Quote { Id = Guid.NewGuid(), Text = "Q3", Author = "A3", Origin = "O3", Categories = ["c3"] }
        ];
    }

    [Fact]
    public async Task GetRandomAsync_ReturnsQuote_AndMarksAsSeen()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var sampleQuotes = CreateSampleQuotes();
        var chosenQuote = sampleQuotes[1]; // Simulate that repository picks Q2 as the random unseen quote

        var quoteRepoMock = new Mock<IQuoteRepository>();
        quoteRepoMock
            .Setup(r => r.GetRandomUnseenAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(chosenQuote);

        var seenRepoMock = new Mock<ISeenRepository>();

        var service = new QuoteService(quoteRepoMock.Object, seenRepoMock.Object);

        // Act
        var result = await service.GetRandomAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(chosenQuote.Id, result!.Id);

        // Verify that we marked the returned quote as seen
        seenRepoMock.Verify(
            r => r.MarkSeenAsync(userId, chosenQuote.Id, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetRandomAsync_WhenNoQuoteAvailable_ReturnsNull()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var quoteRepoMock = new Mock<IQuoteRepository>();
        quoteRepoMock
            .Setup(r => r.GetRandomUnseenAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Quote?)null); // No unseen quotes

        var seenRepoMock = new Mock<ISeenRepository>();

        var service = new QuoteService(quoteRepoMock.Object, seenRepoMock.Object);

        // Act
        var result = await service.GetRandomAsync(userId);

        // Assert
        Assert.Null(result);

        // Ensure nothing attempted to mark seen
        seenRepoMock.Verify(
            r => r.MarkSeenAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task ClearSeenAsync_CallsRepository()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var quoteRepoMock = new Mock<IQuoteRepository>(); 
        var seenRepoMock = new Mock<ISeenRepository>();

        var service = new QuoteService(quoteRepoMock.Object, seenRepoMock.Object);

        // Act
        await service.ClearSeenAsync(userId);

        // Assert
        seenRepoMock.Verify(
            r => r.ClearSeenAsync(userId, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}