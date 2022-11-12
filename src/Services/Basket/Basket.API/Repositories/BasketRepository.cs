using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basket.Api.Data;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.Api.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCashe;

        public BasketRepository(IDistributedCache redisCashe)
        {
            _redisCashe = redisCashe ?? throw new ArgumentNullException(nameof(redisCashe));
        }

        public async Task DeleteBasket(string userName)
        {
            await _redisCashe.RemoveAsync(userName);
        }

        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var basket=await _redisCashe.GetStringAsync(userName);
            if(String.IsNullOrEmpty(basket))
                return null;
            
            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            await _redisCashe.SetStringAsync(basket.UserName,JsonConvert.SerializeObject(basket));

            return await GetBasket(basket.UserName);
        }
    }
}