using AspNetCore.UnitTest.Api.Entities;
using AspNetCore.UnitTest.Api.Models;
using AutoMapper;

namespace AspNetCore.UnitTest.Api.AutoMapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<UserRegisterModel, Users>().ReverseMap();
            CreateMap<UserLoginModel, Users>().ReverseMap();
            CreateMap<UserLoginModel, UserModel>().ReverseMap();
            CreateMap<UserModel, Users>().ReverseMap();
        }
    }
}
