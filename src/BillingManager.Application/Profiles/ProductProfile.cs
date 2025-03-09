using AutoMapper;
using BillingManager.Application.Commands.Products;
using BillingManager.Application.Commands.Products.Create;
using BillingManager.Application.Commands.Products.Update;
using BillingManager.Application.Queries.Products;
using BillingManager.Domain.Entities;

namespace BillingManager.Application.Profiles;

/// <summary>
/// AutoMapper profile for product
/// </summary>
public sealed class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<CreateProductCommand, Product>();
        CreateMap<UpdateProductCommand, Product>();
        CreateMap<Product, ProductCommandResponse>();
        CreateMap<Product, ProductQueryResponse>();
    }
}
