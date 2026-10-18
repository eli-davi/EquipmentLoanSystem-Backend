using AutoMapper;
using EquipmentLoanSystem.Core.DTOs;
using EquipmentLoanSystem.Core.Entities;

namespace EquipmentLoanSystem.API.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Item, ItemDto>();
        }
    }
}