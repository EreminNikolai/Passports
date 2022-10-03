using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Passports.Api.Mappings;

namespace Passports.Api.Models.Dto
{
    public class CreatePassportDto : IMapWith<Passport.Passport>
    {
        [Required]
        public uint Series { get; set; }
        [Required]
        public uint Number { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreatePassportDto, Passport.Passport>()
                .ForMember(t => t.Series, opt => opt.MapFrom(t => t.Series))
                .ForMember(t => t.Number, opt => opt.MapFrom(t => t.Number));
        }
    }
}
