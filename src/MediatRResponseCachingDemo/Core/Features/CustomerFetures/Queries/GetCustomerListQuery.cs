using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Abstractions;
using Core.Entities;
using MediatR;

namespace Core.Features.CustomerFetures.Queries
{
    public class GetCustomerListQuery : IRequest<List<Customer>>, ICacheableMediatrQuery
    {
        public int Id { get; set; }
        public bool BypassCache { get; set; }
        public string CacheKey => $"CustomerList";
        public TimeSpan? SlidingExpiration { get; set; }
    }

    internal class GetCustomerListQueryHandler : IRequestHandler<GetCustomerListQuery, List<Customer>>
    {
        private readonly ICustomerService _customerService;

        public GetCustomerListQueryHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<List<Customer>> Handle(GetCustomerListQuery request, CancellationToken cancellationToken)
        {
            var cutomers = _customerService.GetCustomerList();
            return cutomers.ToList();
        }
    }
}
