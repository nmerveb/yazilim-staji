using SehirRehberi.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SehirRehberi.API.Data
{

    //Controlerlarda kullanmak için.
   public interface IAppRepository
    {
        void Add<T>(T entity)where T:class;
        void Delete<T>(T entity)where T:class;
        bool saveAll();

        List<City> GetCities();
        List<Photo> GetPhotoById(int cityId);
        City GetCityById(int cityId);
        Photo GetPhoto(int id);
    }
}
