using System;
using System.Collections.Generic;

namespace HealthchecksDemo.HealthChecks.Responses
{
    public class HealthCheckReponse
    {
        public string Status { get; set; }
        public IEnumerable<IndividualHealthCheckResponse> HealthChecks { get; set; }
        public TimeSpan HealthCheckDuration { get; set; }
    }
}