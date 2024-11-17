using AutoMapper;
using HouseKeeperApi.Entities;
using HouseKeeperApi.Models;

namespace HouseKeeperApi
{
    public class HouseKeeperMappingProfile : Profile
    {
        public HouseKeeperMappingProfile()
        {
            CreateMap<Equipment, EquipmentDto>().ReverseMap();
            CreateMap<EquipmentDto, Equipment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<House, HouseDto>().ReverseMap();   //get House
            CreateMap<HouseDto, House>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());   // create/update House -ignorowanie ID

            CreateMap<Issue, IssueDto>().ReverseMap();
            CreateMap<IssueDto, Issue>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Message, MessageDto>().ReverseMap();
            CreateMap<MessageDto, Message>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Room, RoomDto>().ReverseMap();
            CreateMap<RoomDto, Room>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<User, UserByIdDto>().ReverseMap();
            CreateMap<UserByIdDto, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

        }
    }
}
