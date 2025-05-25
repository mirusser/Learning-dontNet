using Dapper.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.Infrastructure.Repositories;

public class UnitOfWork(IProductRepository products) : IUnitOfWork
{
    public IProductRepository Products { get; } = products;
}