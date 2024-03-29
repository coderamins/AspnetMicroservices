using System.Net;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Discount.Api.Entities;
using Discount.Api.Repositories;

namespace Discount.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DiscountController : Controller
    {
        private readonly IDiscountRepository _repository;

        public DiscountController(IDiscountRepository repository)
        {
            _repository=repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("{productName}",Name ="GetDiscount")]
        [ProducesResponseType(typeof(Coupon),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> GetDiscoount(string productName)
        {
            var coupon=await _repository.GetDiscount(productName);
            return coupon;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Coupon),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> CreateDiscoount([FromBody] Coupon coupon)
        {
            await _repository.CreateDiscount(coupon);
            return CreatedAtRoute("GetDiscount",new {
                productName=coupon.ProductName
            },coupon);
        }
        
        [HttpPut]
        [ProducesResponseType(typeof(Coupon),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> UpdateDiscoount([FromBody] Coupon coupon)
        {
            return Ok(await _repository.UpdateDiscount(coupon));  
        }

        [HttpDelete("{productName}",Name ="DeleteDiscount")]
        [ProducesResponseType(typeof(Coupon),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> DeleteDiscoount(string productName)
        {
            return Ok(await _repository.DeleteDiscount(productName));  
        }
    }
}