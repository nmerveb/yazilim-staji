﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SehirRehberi.API.Data;
using SehirRehberi.API.Dtos;
using SehirRehberi.API.Models;

namespace SehirRehberi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private IAppRepository _appRepository;

        //IMapper AutoMapper'ın temel interface'idir.
        private IMapper _mapper;


        public CitiesController(IAppRepository appRepository, IMapper mapper)
        {
            _appRepository = appRepository;
            _mapper = mapper;
        }


        //Uygulamanın ana ekranında şehirleri listelemek için kullanıyoruz.
        public ActionResult GetCities()
        {
            //var cities = _appRepository.GetCities().Select(c=>c.Name); >>>>Şeklinde bir sorgu yapsaydık listeyi değil name değerlerini döndürürdü.
            //var cities = _appRepository.GetCities();                   >>>>Sorgusu komple listeyi döndürür.
            //var cities = _appRepository.GetCities().Select(c=> new CityForListDto {Description = c.Description, Name=c.Name, id=c.Id,
            //   PhotoUrl =c.Photos.FirstOrDefault(p=>p.IsMain==true).Url}).ToList();   >>>>AutoMapper kullanılmazsa.
            var cities = _appRepository.GetCities();
            var citiesToReturn = _mapper.Map<List<CityForListDto>>(cities);
            return Ok(citiesToReturn);

        }
        [HttpPost]
        [Route("add")] //url'ye api/cities/add yazılırsa çağrılacak.
        public ActionResult Add([FromBody] City city)
        {
            _appRepository.Add(city);
            _appRepository.saveAll();
            return Ok(city);
        }

        [HttpGet]
        [Route("detail")]
        public ActionResult GetCityById(int id)
        {
            var city = _appRepository.GetCityById(id);
            var cityToReturn = _mapper.Map<CityForDetailDto>(city);
            return Ok(cityToReturn);

        }

        //Fotoğrafların çağrılması.
        [HttpGet]
        [Route("Photos")]
        public ActionResult GetPhotosByCity(int cityId)
        {
            var photos = _appRepository.GetPhotoById(cityId);
            return Ok(photos);
        }



    }
}