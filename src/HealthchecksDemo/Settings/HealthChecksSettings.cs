namespace HealthchecksDemo.Settings
{
    public class HealthChecksSettings
    {
        public UIConfiguration UIConfiguration { get; set; } = null!;
    }

    public class UIConfiguration
    {
        public int ApiRequestsConcurrency { get; set; }
        public int MaxNumberOfEntriesPerEndpoint { get; set; }
        public int EvaluationTimeInSeconds { get; set; }
    }
}