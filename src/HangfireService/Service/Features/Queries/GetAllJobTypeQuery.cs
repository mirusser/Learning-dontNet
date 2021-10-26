using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HangfireService.Models;
using MediatR;
using Newtonsoft.Json;

namespace HangfireService.Features.Queries
{
    public class GetAllJobTypeQuery : IRequest<IEnumerable<JobTypeDto>>
    {
    }

    public class GetAllJobTypeHandler : IRequestHandler<GetAllJobTypeQuery, IEnumerable<JobTypeDto>>
    {
        public async Task<IEnumerable<JobTypeDto>> Handle(GetAllJobTypeQuery request, CancellationToken cancellationToken)
        {
            var jobTypes = (JobType[])Enum.GetValues(typeof(JobType));

            var result = jobTypes
               .Select(j => new JobTypeDto { Value = (int)j, Name = j.ToString() });

            return await Task.FromResult(result);
        }
    }

    public class JobTypeDto
    {
        public int Value { get; set; }
        public string? Name { get; set; }
    }
}