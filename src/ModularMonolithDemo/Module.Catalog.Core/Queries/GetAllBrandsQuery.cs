using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Module.Catalog.Core.Abstractions;
using Module.Catalog.Core.Entities;

namespace Module.Catalog.Core.Queries;

public class GetAllBrandsQuery : IRequest<IEnumerable<Brand>>
{
}

internal class BrandQueryHandler : IRequestHandler<GetAllBrandsQuery, IEnumerable<Brand>>
{
    private readonly ICatalogDbContext _context;

    public BrandQueryHandler(ICatalogDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Brand>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
    {
        var brands = await _context.Brands.OrderBy(x => x.Id).ToListAsync(cancellationToken: cancellationToken);
        if (brands == null) throw new Exception("Brands Not Found!");
        return brands;
    }
}

