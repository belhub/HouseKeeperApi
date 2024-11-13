using AutoMapper;
using HouseKeeperApi.Entities;
using HouseKeeperApi.Models;

namespace HouseKeeperApi
{
    public class HouseKeeperMappingProfile : Profile
    {
        public HouseKeeperMappingProfile()
        {
            CreateMap<Equipment, EquipmentDto>();
            CreateMap<House, HouseDto>();   //get House
            CreateMap<HouseDto, House>().ForMember(dest => dest.Id, opt => opt.Ignore());   // create/update House -ignorowanie ID
            CreateMap<Issue, IssueDto>();
            CreateMap<Message, MessageDto>();
            CreateMap<Room, RoomDto>();
        }
    }
}
