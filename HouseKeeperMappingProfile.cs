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

            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.SenderName,
                   opt => opt.MapFrom(src => $"{src.Sender.Name} {src.Sender.Surname}"))
                .ForMember(dest => dest.SenderId,
                   opt => opt.MapFrom(src => src.Sender.Id));
            CreateMap<MessageDto, Message>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Room, RoomDto>().ReverseMap();
            CreateMap<RoomDto, Room>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<User, UserByIdDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.RoleName));
            CreateMap<UserByIdDto, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Transaction, TransactionDto>().ReverseMap();
            CreateMap<TransactionDto, Transaction>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());


            CreateMap<Schedule, ScheduleDto>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => $"{src.User.Name} {src.User.Surname}"));
            CreateMap<ScheduleDto, Schedule>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Notification, NotificationDto>().ReverseMap();
            CreateMap<NotificationDto, Notification>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
