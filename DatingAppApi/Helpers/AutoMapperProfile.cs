using AutoMapper;
using DatingAppApi.Dtos;
using DatingAppApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingAppApi.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserForListDto>()
                .ForMember(dest => dest.PhotoUrl, opt =>
                {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                })
                .ForMember(dest => dest.Age, opt =>
                {
                    opt.ResolveUsing(d => d.DateOfBirth.CalculatAge());
                });

            CreateMap<User, UserForDetailedDto>()
                 .ForMember(dest => dest.PhotoUrl, opt =>
                 {
                     opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                 })
                .ForMember(dest => dest.Age, opt =>
                {
                    opt.ResolveUsing(d => d.DateOfBirth.CalculatAge());
                });


            CreateMap<Photo, PhotosForDetailedDto>();

            CreateMap<UserForUpdateDto, User>();

            CreateMap<PhotoForCreationDto, Photo>();

            CreateMap<Photo, PhotoForReturnDto>();

            CreateMap<UserForRegisterDto, User>();
        }
    }
}
