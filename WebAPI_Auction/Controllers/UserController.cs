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
using AutoMapper;

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
                User bll_user = UOperations.CheckUser(login, password);
                UserModel user = Mapper.Map<User, UserModel>(bll_user);
                if (user != null)
                    return Ok(user);
                else return BadRequest("Please, check correctness of login and password");
            }
        }

        [HttpGet]
        [Route("api/user")]
        public IEnumerable<UserModel> GetUsers()
        {
            IEnumerable<User> users = UOperations.GetUsers();
            IEnumerable<UserModel> ui_users = Mapper.Map<IEnumerable<User>, IEnumerable<UserModel>>(users);
            return ui_users;
        }

        [HttpPost]
        [Route("api/user/newUser")]
        public IHttpActionResult PostUser(UserModel _user)
        {
            if (string.IsNullOrWhiteSpace(_user.Login) || string.IsNullOrWhiteSpace(_user.Password)
                || string.IsNullOrWhiteSpace(_user.Name) || string.IsNullOrWhiteSpace(_user.Surname)
                || string.IsNullOrWhiteSpace(_user.Patronymic) || _user.PhoneNumber == 0 
                || string.IsNullOrWhiteSpace(_user.Passport))
            {
                return BadRequest("Please, fill all fields");
            }
            else if (UOperations.CheckUser(_user.Login))
                return BadRequest("This login already registered");
            else if (UOperations.CheckUser(_user.Name, _user.Surname, _user.Patronymic))
                return BadRequest("Such person already registered");
            else
            {
                User user = Mapper.Map<UserModel, User>(_user);
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

