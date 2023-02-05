using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discount.Grpc.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;
using Dapper;

namespace Discount.Grpc.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using var connection =new NpgsqlConnection(
                _configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            
            var affected=
            await connection.ExecuteAsync
                ("INSERT INTO Coupon (ProductName,Description,Amount) Values (@ProductName,@Description,@Amount)",
                    new {
                        ProductName=coupon.ProductName,
                        Description=coupon.Description,
                        Amount=coupon.Amount
                    });

            if(affected==0)
                return false;

            return true;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            using var connection =new NpgsqlConnection(
                _configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            
            var affected=await connection.ExecuteAsync
                ("DELETE FROM COUPON WHERE ProductName=@ProductName",new {ProductName=productName});

            if(affected==0)
                return false;

            return true;
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            using var connection =new NpgsqlConnection(
                _configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            
            var coupon=await connection.QueryFirstOrDefaultAsync
            <Coupon>("SELECT * FROM COUPON WHERE ProductName=@ProductName",new {ProductName=productName});

            if(coupon==null)
                return new Coupon{
                    ProductName="No Discount",
                    Amount=0,
                    Description="No Discount Desc"
                };

            return coupon;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection =new NpgsqlConnection(
                _configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            
            var affected=
            await connection.ExecuteAsync
                ("UPDATE Coupon set ProductName=@ProductName,Description=@Description,Amount=@Amount WHERE Id=@Id",
                    new {
                        Id=coupon.Id,
                        ProductName=coupon.ProductName,
                        Description=coupon.Description,
                        Amount=coupon.Amount
                    });

            if(affected==0)
                return false;

            return true;
        }
    }
}
