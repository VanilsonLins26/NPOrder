
using AutoMapper;
using Microsoft.OpenApi.Extensions;
using NP_Encomendas_BackEnd.DTOs.Request;
using NP_Encomendas_BackEnd.DTOs.Response;
using NP_Encomendas_BackEnd.Models;

namespace NP_Encomendas_BackEnd.DTOs.Mapping;

public class DTOMapping : Profile
{
    public DTOMapping()
    {
        CreateMap<Product, ProductResponseDto>().ReverseMap();
        CreateMap<Product, ProductRequestDto>().ReverseMap();
        CreateMap<Cart, CartResponseDTO>().
            ForMember(c => c.Total, opt => opt.MapFrom(src => src.CartItems.Sum(ci => ci.TotalPrice)));
        CreateMap<Cart, CartRequestDTO>().ReverseMap();
        CreateMap<CartItem, CartItemRequestDTO>().ReverseMap();
        CreateMap<CartItem, CartItemResponseDTO>().ReverseMap();
        CreateMap<CartHeader, CartHeaderResponseDTO>().ReverseMap();
        CreateMap<CartHeader, CartHeaderRequestDTO>().ReverseMap();
        CreateMap<Order, OrderRequestDTO>().ReverseMap();
        CreateMap<Order, OrderResponseDTO>().
            ForMember(o => o.StatusName, opt => opt.MapFrom(src => src.Status.GetDisplayName()));
        CreateMap<OrderResponseDTO, Order>();
        CreateMap<Order, PaymentOrderDTO>().ReverseMap();
        CreateMap<OrderItem, OrderItemRequestDTO>().ReverseMap();
        CreateMap<OrderItem, OrderItemResponseDTO>().ReverseMap();
        CreateMap<Address, AddressResponseDTO>().ReverseMap();
        CreateMap<Address, AddressRequestDTO>().ReverseMap();
        CreateMap<PromotionPrice, PromotionPriceResponseDTO>().ReverseMap();
        CreateMap<PromotionPrice, PromotionPriceRequestDTO>().ReverseMap();
        CreateMap<PaymentEntity, PaymentRequestDTO>().ReverseMap();
        CreateMap<PaymentEntity, PaymentResponseDTO>().ReverseMap();

    }
}
