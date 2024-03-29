﻿using System.Threading.Tasks;
using Shared.DbModels;

namespace OrderService.Providers
{
    public interface IOrderCreatorProvider
    {
        Task<int> CreateAsync(OrderDetail orderDetail);
    }
}