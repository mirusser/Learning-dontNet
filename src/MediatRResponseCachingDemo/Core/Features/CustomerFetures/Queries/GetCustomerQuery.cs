using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Abstractions;
using Core.Entities;
using MediatR;

namespace Core.Features.CustomerFeatures.Queries
{
    public class GetCustomerQuery : IRequest<Customer?>, ICacheableMediatrQuery
    {
        public int Id { get; set; }
        public bool BypassCache { get; set; }
        public string CacheKey => $"Customer-{Id}";
        public TimeSpan? SlidingExpiration { get; set; }
    }

    internal class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, Customer?>
    {
        private readonly ICustomerService _customerService;

        public GetCustomerQueryHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<Customer?> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            var customer = _customerService.GetCustomer(request.Id);
            return customer;
        }
    }
}
