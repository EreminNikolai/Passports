using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Passports.Api.Mappings;

namespace Passports.Api.Models.Dto
{
    public class PassportDto : IMapWith<Passport.Passport>
    {
        public int Id { get; set; }
        public uint Series { get; set; }
        public uint Number { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PassportDto, Passport.Passport>()
                .ForMember(t => t.Id, opt => opt.MapFrom(t => t.Id))
                .ForMember(t => t.Series, opt => opt.MapFrom(t => t.Series))
                .ForMember(t => t.Number, opt => opt.MapFrom(t => t.Number));

            profile.CreateMap<Passport.Passport, PassportDto>()
                .ForMember(t => t.Id, opt => opt.MapFrom(t => t.Id))
                .ForMember(t => t.Series, opt => opt.MapFrom(t => t.Series))
                .ForMember(t => t.Number, opt => opt.MapFrom(t => t.Number));
        }
    }
}
