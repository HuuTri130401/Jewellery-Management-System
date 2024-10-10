using AutoMapper;
using THT.JMS.Application.CreateModels;
using THT.JMS.Application.ResponseModels;
using THT.JMS.Application.UpdateModels;
using THT.JMS.Domain.Entities;

namespace THT.JMS.API
{
    public class AutoMapper : Profile
    {
        public AutoMapper() 
        {
            //Người dùng
            CreateMap<UserModel, Users>().ReverseMap();
            CreateMap<UserCreate, Users>().ReverseMap();
            CreateMap<UserUpdate, Users>().ReverseMap();
            //CreateMap<PagedList<UserModel>, PagedList<Users>>().ReverseMap();

        }
    }
}
