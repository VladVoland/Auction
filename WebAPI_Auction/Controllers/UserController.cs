using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Security.Claims;
using BLL;
using Newtonsoft.Json.Linq;
using Ninject;
using NinjectConfiguration;

namespace OnlineAuction.Controllers
{
    public class UserController : ApiController
    {
        IKernel ninjectKernel;
        IUser_Operations UOperations;
        public UserController()
        {
            ninjectKernel = new StandardKernel(new NinjectConfig());
            UOperations = ninjectKernel.Get<IUser_Operations>();
        }

        [HttpGet]
        [Route("api/user/{login}/{password}")]
        public IHttpActionResult SignIn(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                return BadRequest("Please, enter login and password");
            else
            {
                User userId = UOperations.CheckUser(login, password);
                if (userId != null)
                    return Ok(userId);
                else return BadRequest("Please, check correctness of login and password");
            }
        }

        [HttpGet]
        [Route("api/user")]
        public IEnumerable<User> GetUsers()
        {
            return UOperations.GetUsers();
        }

        [HttpPost]
        [Route("api/user/newUser")]
        public IHttpActionResult PostUser(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Login) || string.IsNullOrWhiteSpace(user.Password)
                || string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Surname)
                || string.IsNullOrWhiteSpace(user.Patronymic) || user.PhoneNumber == 0 || string.IsNullOrWhiteSpace(user.Passport))
            {
                return BadRequest("Please, fill all fields");
            }
            else if (UOperations.CheckUser(user.Login)) return BadRequest("This login already registered");
            else if (UOperations.CheckUser(user.Name, user.Surname, user.Patronymic)) return BadRequest("Such person already registered");
            else
            {
                UOperations.SaveUser(user);
                return Ok();
            }
        }
        [HttpDelete]
        [Route("api/user/delete/{id}")]
        public IHttpActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid student id");
            UOperations.deleteUser(id);
            return Ok();
        }
    }
}

