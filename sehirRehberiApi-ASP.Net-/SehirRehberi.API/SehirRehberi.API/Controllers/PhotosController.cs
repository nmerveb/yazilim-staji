using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SehirRehberi.API.Data;
using SehirRehberi.API.Dtos;
using SehirRehberi.API.Helpers;
using SehirRehberi.API.Models;

namespace SehirRehberi.API.Controllers
{
    [Route("api/cities/{cityId}/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private IAppRepository _appRepository;
        private IMapper _mapper;
        private IOptions<CloudinarySettings> _cloudinaryconfig;

        Cloudinary _cloudinary;
        public PhotosController(IAppRepository appRepository, IMapper mapper, IOptions<CloudinarySettings> cloudinaryconfig)
        {
            _appRepository = appRepository;
            _mapper = mapper;
            _cloudinaryconfig = cloudinaryconfig;

            //Cloudinary hesabını aktifleştirmek için 
            Account account = new Account(_cloudinaryconfig.Value.CloudName, _cloudinaryconfig.Value.ApiKey, _cloudinaryconfig.Value.ApiSecret);

            //Cloudinary'i başlatmak için.
            _cloudinary = new Cloudinary(account);
        }

        [HttpPost]
        public ActionResult AddPhotoForCity(int cityId,[FromBody]PhotoForCreationDto photoForCreationDto)
        {
            var city = _appRepository.GetCityById(cityId);
            //Url'in manuel değiştirilme ihtimaline karşı

            if (city == null)
            {
                return BadRequest("Could not find city");
            }

            //Hangi kullanıcının fotoğrafı olduğunu belirtmek için
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            //Kullanıcın farklı birisine ait fotoğraf eklemek istemesi durumunda.
            if (currentUserId != city.UserId)
            {
                return Unauthorized();
            }

            //Gönderilen dosyanın kaydedilmek üzere okunması işlemi.
            var file = photoForCreationDto.File;
            var uploadResult = new ImageUploadResult();
            //Dosya var mı kontrolü
            if(file.Length>0)
            {
                //Fotoğrafın kaydı
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams {
                        File = new FileDescription(file.Name, stream)   
                    };
                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }
            //Veritabanına kayıt
            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoForCreationDto);
            photo.city = city;
            if (!city.Photos.Any(p=>p.IsMain))
            {
                photo.IsMain = true;
            }
            if (_appRepository.saveAll())
            {
                var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute("GetPhoto", new { id = photo.Id }, photoToReturn);
            }
            return BadRequest("Could not Added");
         }
        
        [HttpGet("{id}", Name="GetPhoto")]
        public ActionResult GetPhoto(int id)
        {
            var photoFromDb = _appRepository.GetPhoto(id);
            var photo = _mapper.Map<PhotoForReturnDto>(photoFromDb);
            return Ok(photo);
        }
    }
}