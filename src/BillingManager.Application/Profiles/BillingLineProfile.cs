using AutoMapper;
using BillingManager.Domain.Entities;
using BillingManager.Domain.HttpClient;

namespace BillingManager.Application.Profiles;

/// <summary>
/// AutoMapper profile for billing lines
/// </summary>
public class BillingLineProfile : Profile
{
    public BillingLineProfile()
    {
        CreateMap<BillingLineApiResponse, BillingLine>()
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.Subtotal))
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId));
    }
}