using Dapper.Application.Interfaces;
using Dapper.Core.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Dapper.Infrastructure.Repositories;

public class ProductRepository(IConfiguration configuration) : IProductRepository
{
    private readonly string connectionString = configuration.GetConnectionString("DefaultConnection");

    public async Task<int> AddAsync(Product entity)
    {
        entity.AddedOn = DateTime.Now;
        var sql = $"""
                   Insert into Products (
                        {nameof(Product.Name)}, 
                        {nameof(Product.Description)}, 
                        {nameof(Product.Barcode)}, 
                        {nameof(Product.Rate)}, 
                        {nameof(Product.AddedOn)})
                                values (
                        @{nameof(Product.Name)}, 
                        @{nameof(Product.Description)}, 
                        @{nameof(Product.Barcode)}, 
                        @{nameof(Product.Rate)}, 
                        @{nameof(Product.AddedOn)})
                   """;

        await using var connection = new SqlConnection(connectionString);
        connection.Open();
        var result = await connection.ExecuteAsync(sql, entity);

        return result;
    }

    public async Task<int> DeleteAsync(int id)
    {
        var sql = $"delete from Products where {nameof(Product.Id)} = @{nameof(Product.Id)}";
        await using var connection = new SqlConnection(connectionString);
        connection.Open();
        var result = await connection.ExecuteAsync(sql, new { Id = id });

        return result;
    }

    public async Task<IReadOnlyList<Product>> GetAllAsync()
    {
        var sql = $"select * from Products";
        await using var connection = new SqlConnection(connectionString);
        connection.Open();
        var result = await connection.QueryAsync<Product>(sql);

        return result.ToList();
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        var sql = $"select * from Products where {nameof(Product.Id)} = @{nameof(Product.Id)}";
        await using var connection = new SqlConnection(connectionString);
        connection.Open();
        var result = await connection.QuerySingleOrDefaultAsync<Product>(sql, new { Id = id });

        return result;
    }

    public async Task<int> UpdateAsync(Product entity)
    {
        entity.ModifiedOn = DateTime.Now;
        var sql = $"""
                   UPDATE Products 
                        SET 
                        {nameof(Product.Name)} = @{nameof(Product.Name)}, 
                        {nameof(Product.Description)} = @{nameof(Product.Description)}, 
                        {nameof(Product.Barcode)} = @{nameof(Product.Barcode)}, 
                        {nameof(Product.Rate)} = @{nameof(Product.Rate)}, 
                        {nameof(Product.ModifiedOn)} = @{nameof(Product.ModifiedOn)}
                        WHERE {nameof(Product.Id)} = @{nameof(Product.Id)}
                   """;

        await using var connection = new SqlConnection(connectionString);
        connection.Open();
        var result = await connection.ExecuteAsync(sql, entity);

        return result;
    }
}