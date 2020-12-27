using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SehirRehberi.API.Data;
using SehirRehberi.API.Dtos;
using SehirRehberi.API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Newtonsoft.Json;

namespace SehirRehberi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthRepository _authRepository;
        private IConfiguration _configuration;

        public AuthController(IAuthRepository authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }


        //Kayıt
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserForRegisterDto userForRegisterDto)
        {
            if(await _authRepository.UserExists(userForRegisterDto.UserName))
            {
                ModelState.AddModelError("UserName", "UserName alredy exists");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userToCreate = new User
            {
                Username = userForRegisterDto.UserName
            };

            var createdUser = await _authRepository.Register(userToCreate, userForRegisterDto.Password);
          
            // Aşağıdaki kod oluşturulduya karşılık gelir.
            return StatusCode(201);
        }


        //Login
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserForLoginDto userForLoginDto)
        {
            //Veri tabanında istenilen kullanıcının olup olmadığı bilgisini döner.
            var user = await _authRepository.Login(userForLoginDto.UserName, userForLoginDto.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            //Bu aşamadan sonra kullanıcı sistemde var ise ona bir token göndeririz ve o token geçerliliğini yitirene kadar kullanıcı o token ile işlemlerini gerçekleştirir.

            //Öncelikle token işlemini yapacak bir handler tanımlarız.
            var tokenHandler = new JwtSecurityTokenHandler();
            //Token'ı üretebilmek için AppSettings deki token keyine ulaşmamız gerekir.
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Token").Value);
            //Ürettiğimiz token'ın içinde neler olacağını tanımlarız.
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    //Tokenda tutmak istediğimiz temel bilgileri tutar.
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name,user.Username)
                }),
                //Token'ın geçerlilik süresini belirler.
                Expires = DateTime.Now.AddDays(1),
                //Token'ı üretmek için kullandığımız key'i ve hangi algoritmayı kullandığımızı göstermemiz gerekir.
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)

            };

            //Token'ı üretmek
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = JsonConvert.SerializeObject(tokenHandler.WriteToken(token));

            return Ok(tokenString);
        }


    }
}