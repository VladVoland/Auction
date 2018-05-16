using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BLL;
using Ninject;
using NinjectConfiguration;

namespace OnlineAuction.Controllers
{
    public class CategoryController : ApiController
    {
        IKernel ninjectKernel;
        ICategory_Operations COperations;
        public CategoryController()
        {
            ninjectKernel = new StandardKernel(new NinjectConfig());
            COperations = ninjectKernel.Get<ICategory_Operations>();
        }

        [HttpGet]
        [Route("api/category")]
        public IEnumerable<Category> GetCategories()
        {
            return COperations.GetCategories();
        }

        [HttpPost]
        [Route("api/saveCategory/{name}")]
        public IHttpActionResult PostCategory(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Please, enter category name");
            else
            {
                COperations.SaveCategory(name);
                return Ok("Success");
            }
        }

        [HttpDelete]
        [Route("api/deleteCaregory/{id}")]
        public void Delete(int id)
        {
            COperations.deleteCategory(id);
        }
    }
}
