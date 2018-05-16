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
    public class SubcategoryController : ApiController
    {
        IKernel ninjectKernel;
        ISubcategory_Operations SOperations;
        public SubcategoryController()
        {
            ninjectKernel = new StandardKernel(new NinjectConfig());
            SOperations = ninjectKernel.Get<ISubcategory_Operations>();
        }

        [HttpGet]
        public IEnumerable<Subcategory> GetSubcategory()
        {
            return SOperations.GetSubcategories();
        }
        [HttpGet]
        [Route("api/subcategory/get/{categoryName}")]
        public IEnumerable<Subcategory> GetSubcategoryByCateg(string categoryName)
        {
            return SOperations.GetSubcategoriesByCateg(categoryName);
        }

        [HttpPost]
        [Route("api/saveSubcategory/{Categoryname}/{Subcategoryname}")]
        public IHttpActionResult PostSubcategory(string Categoryname, string Subcategoryname)
        {
            if (string.IsNullOrWhiteSpace(Subcategoryname) || string.IsNullOrWhiteSpace(Categoryname))
                return BadRequest("Please, enter subcategory name");
            else
            {
                SOperations.SaveSubcategory(Subcategoryname, Categoryname);
                return Ok();
            }
        }

        [HttpDelete]
        [Route("api/deleteSubcategory/{id}")]
        public void Delete(int id)
        {
            SOperations.deleteSubcategory(id);
        }
    }
}
