using CQRS_with_MediatR.Context;
using CQRS_with_MediatR.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS_with_MediatR.Features.ProductFeatures.Queries
{
    public class GetProductByIdQuery : IRequest<Product>
    {
        public int Id { get; set; }
    }

    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product>
    {
        private readonly IApplicationContext _context;

        public GetProductByIdQueryHandler(IApplicationContext context)
        {
            _context = context;
        }

        public async Task<Product> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            var product = await _context.Products.Where(a => a.Id == query.Id).FirstOrDefaultAsync();

            if (product == null) return null;

            return product;
        }
    }
}
