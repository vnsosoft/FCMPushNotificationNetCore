using AutoMapper;
using FCMPushNotification.Entities;
using FCMPushNotification.Services.Models.Account;

namespace FCMPushNotification.Services.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AccountResponse, Account>().ReverseMap();
        }
    }
}
