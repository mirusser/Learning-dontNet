using System;

namespace HealthchecksDemo.HealthChecks.Responses
{
    public class IndividualHealthCheckResponse
    {
        public string? Status { get; set; }
        public string? Component { get; set; }
        public string? Description { get; set; }
        public Exception? Exception { get; set; }
    }
}