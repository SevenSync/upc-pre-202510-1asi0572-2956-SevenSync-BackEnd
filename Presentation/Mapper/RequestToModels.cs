using AutoMapper;
using Domain.IAM.Models.Aggregates;
using Domain.IAM.Models.Commands;
using Domain.IAM.Models.ValueObjects.User;
using Presentation.IAM.Response;

namespace Presentation.Mapper;

public class RequestToModels : Profile
{
    public RequestToModels()
    {
        //  @AuthenticationRequest to @UserCredentials
        CreateMap<UserRegistrationCommand, User>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => new FullName(src.Name, src.LastName)))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => new Email(src.Email)));
        
        //  @RefreshTokenRequest to @RefreshTokenModel
        CreateMap<RefreshTokenCommand, CreateRefreshTokenCommand>()
            .ForMember(dest => dest.ExpiredToken, opt => opt.MapFrom(src => src.ExpiredToken))
            .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.RefreshToken));
        
        //  @User to @UserResponse
        CreateMap<User, UserResponse>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId.Value))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName.Name))
            .ForMember(dest => dest.LastNames, opt => opt.MapFrom(src => src.FullName.LastNames));
    }
}