using AutoMapper;
using SehirRehberi.API.Dtos;
using SehirRehberi.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SehirRehberi.API.Helpers
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            //Hangi Sınıflar için map yapacağımızı tanımlarız.
            //İsmi eşleşenleri direkt map eder.
            CreateMap<City, CityForListDto>().ForMember(dest=>dest.PhotoUrl,opt=>{
                //Alınacak dosyanın kaynağı belirtildi.
                opt.MapFrom(src=>src.Photos.FirstOrDefault(p=>p.IsMain).Url);
            });

            //Verini  bir parçası değil de direkt kendisi çekileceği için yeterli tanımlama.
            CreateMap<City, CityForDetailDto>();
            //Cloudinary'deki fotoğraflar için.
            CreateMap<Photo, PhotoForCreationDto>();

            CreateMap<PhotoForReturnDto,Photo>();

        }

    }
}
