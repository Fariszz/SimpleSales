using AutoMapper;
using Backend.Models;
using Backend.Utils.Request;
using Backend.Utils.Response;

namespace Backend.Data;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User, AuthenticateResponse>();
        CreateMap<RegisterRequest, User>();
    }
}