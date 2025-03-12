using AutoMapper;
using BillingManager.Domain.Entities;
using BillingManager.Domain.HttpClient;

namespace BillingManager.Application.Profiles;

public class BillingProfile : Profile
{
    public BillingProfile()
    {
        CreateMap<BillingApiResponse, Billing>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
            .ForMember(dest => dest.InvoiceNumber, opt => opt.MapFrom(src => src.InvoiceNumber))
            .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueDate))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Customer.Id))
            .ForMember(dest => dest.Customer, opt => opt.Ignore());
    }
}