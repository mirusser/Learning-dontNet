namespace Loans.Domain.Applications
{
    public interface ICreditScorer
    {
        int Score { get; }
        ScoreResult ScoreResult { get; }
        int Count { get; set; }

        void CalculateScore(string applicantName, string applicantAddress);
    }
}