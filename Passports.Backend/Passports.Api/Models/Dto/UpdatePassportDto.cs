using AutoMapper;
using Passports.Api.Mappings;

namespace Passports.Api.Models.Dto
{
    public class UpdatePassportDto : IMapWith<Passport.Passport>
    {
        public Guid Id { get; set; }
        public uint Series { get; set; }
        public uint Number { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdatePassportDto, Passport.Passport>()
                .ForMember(t => t.Id, opt => opt.MapFrom(t => t.Id))
                .ForMember(t => t.Series, opt => opt.MapFrom(t => t.Series))
                .ForMember(t => t.Number, opt => opt.MapFrom(t => t.Number));
        }
    }
}
