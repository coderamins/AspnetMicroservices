using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;

namespace Discount.Grpc.Mapper
{
    public class DiscouontProfile:Profile
    {
        public DiscouontProfile()
        {
            CreateMap<Coupon, CouponModel>().ReverseMap();
        }
    }
}
