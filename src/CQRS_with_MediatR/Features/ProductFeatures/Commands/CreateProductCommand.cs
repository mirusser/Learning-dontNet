using CQRS_with_MediatR.Context;
using CQRS_with_MediatR.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS_with_MediatR.Features.ProductFeatures.Commands
{
    public class CreateProductCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        public decimal BuyingPrice { get; set; }
        public decimal Rate { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IApplicationContext _context;
        public CreateProductCommandHandler(IApplicationContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            Product product = new()
            {
                Barcode = command.Barcode,
                Name = command.Name,
                BuyingPrice = command.BuyingPrice,
                Rate = command.Rate,
                Description = command.Description
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product.Id;
        }
    }
}
