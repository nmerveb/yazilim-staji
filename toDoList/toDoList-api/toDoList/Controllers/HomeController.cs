using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using toDoList.Data;
using toDoList.Models;

namespace toDoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private IAppRepository _appRepository;
        public  HomeController(IAppRepository appRepository)
        {
            _appRepository = appRepository;
        }


        [HttpGet]
        public ActionResult GetLists(int userId)
        {
            var lists = _appRepository.GetListsByUserId(userId);
             return Ok(lists);
        }


        [HttpPost]
        public ActionResult addPendings([FromBody] Pending pending)
        {

            var user = _appRepository.GetUserById(pending.UserId);

            if (user == null)
            {
                return BadRequest("Could not find user");
            }
            else
            {
                _appRepository.Add(pending);
                _appRepository.saveAll();
                return Ok(pending);
            }

        }

        [HttpPut]
        public ActionResult updateLists(string listname, string comefrom,[FromBody] GeciciEleman geciciEleman)
        {
            var user = _appRepository.GetUserById(geciciEleman.UserId);

            if (user == null)
            {
                return BadRequest("Could not find user");
            }
            else
            {
                if (listname == comefrom)
                {
                    return BadRequest("Aynı liste");
                }
                else
                {
                    if (listname == "cdk-drop-list-0" && comefrom != null)
                    {
                        var inprogress = _appRepository.GetinProgressListById(geciciEleman.Id);
                        Pending pending = new Pending { todo = inprogress.todo, UserId = inprogress.UserId };
                        _appRepository.Add<Pending>(pending); //Pending'e ekle.
                        _appRepository.Delete<inProgress>(inprogress); //inProgress'ten sil.
                        _appRepository.saveAll();
                        return Ok();
                    }
                    else if (listname == "cdk-drop-list-1" && comefrom != null)
                    {
                        if (comefrom == "cdk-drop-list-0")
                        {
                            var pending = _appRepository.GetPendingListById(geciciEleman.Id);
                            inProgress inprogress = new inProgress { todo = pending.todo, UserId = pending.UserId };
                            _appRepository.Add<inProgress>(inprogress); //inProgress'e  ekle.
                            _appRepository.Delete<Pending>(pending); //Pendings'ten sil.
                            _appRepository.saveAll();
                            return Ok();
                        }
                        else if (comefrom == "cdk-drop-list-2")
                        {
                            var done = _appRepository.GetDoneListById(geciciEleman.Id);
                            inProgress inprogress = new inProgress { todo = done.todo, UserId = done.UserId };
                            _appRepository.Add<inProgress>(inprogress); //inProgress'e  ekle.
                            _appRepository.Delete<Done>(done); //Done'dan sil.
                            _appRepository.saveAll();
                            return Ok();
                        }
                    }
                    else if (listname == "cdk-drop-list-2" && comefrom != null)
                    {
                        var inprogress = _appRepository.GetinProgressListById(geciciEleman.Id);
                        Done done = new Done { todo = inprogress.todo, UserId = inprogress.UserId };
                        _appRepository.Add<Done>(done); //inProgress'e  ekle.
                        _appRepository.Delete<inProgress>(inprogress);
                        _appRepository.saveAll();
                        return Ok();
                    }
                }
               
                return Ok();
            }

        }

        [HttpDelete]
        public ActionResult delete(int Id, string listname)
        {
            if (listname == "Pendings")
            {
                if (_appRepository.GetPendingListById(Id) != null)
                {
                    _appRepository.Delete<Pending>(_appRepository.GetPendingListById(Id));
                    _appRepository.saveAll();
                    return Ok();
                }
                return BadRequest("Silme işlemi başarısız!");
            } else if (listname == "inProgress")
            {
                if (_appRepository.GetinProgressListById(Id) != null)
                {
                    _appRepository.Delete<inProgress>(_appRepository.GetinProgressListById(Id));
                    _appRepository.saveAll();
                    return Ok();
                }
                return BadRequest("Silme işlemi başarısız!");
            }
            else if(listname== "Done")
            {
                if (_appRepository.GetDoneListById(Id) != null)
                {
                    _appRepository.Delete<Done>(_appRepository.GetDoneListById(Id));
                    _appRepository.saveAll();
                    return Ok();
                }
                return BadRequest("Silme işlemi başarısız!");
            }
           
          return BadRequest("Silme işlemi başarısız!"); 
           
        }
    }
}