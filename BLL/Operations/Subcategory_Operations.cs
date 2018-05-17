using DAL;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Ninject;

namespace BLL
{
    public class Subcategory_Operations : ISubcategory_Operations
    {
        IKernel ninjectKernel;
        public IUnitOfWork uow { get; set; }
        public Subcategory_Operations()
        {
            ninjectKernel = new StandardKernel();
            ninjectKernel.Bind<IUnitOfWork>().To<UnitOfWork>();
            this.uow = ninjectKernel.Get<IUnitOfWork>();
        }
        public List<Subcategory> GetSubcategories()
        {
            List<Subcategory> subcategories = new List<Subcategory>();
            IEnumerable<DB_Subcategory> dbsubcategories = uow.Subcategories.GetWithInclude(s => s.Category);
            subcategories = Mapper.Map<IEnumerable<DB_Subcategory>, List<Subcategory>>(dbsubcategories);
            return subcategories;
        }

        public IEnumerable<Subcategory> GetSubcategoriesByCateg(string categoryName)
        {
            List<Subcategory> subcategories = new List<Subcategory>();
            IEnumerable<DB_Subcategory> dbsubcategories = uow.Subcategories.GetWithInclude(s => s.Category);
            foreach (DB_Subcategory subc in dbsubcategories)
            {
                if (subc.Category.Name == categoryName)
                    subcategories.Add(Mapper.Map<DB_Subcategory, Subcategory>(subc));
            }
            return subcategories;
        }

        public void SaveSubcategory(string SubcategoryName, string CategoryName)
        {
            DB_Category categ = null;
            IEnumerable<DB_Category> categories = uow.Categories.Get();
            foreach (DB_Category c in categories)
            {
                if (c.Name == CategoryName) categ = c;
            }

            DB_Subcategory subcateg = new DB_Subcategory { Name = SubcategoryName, Category = categ };
            uow.Subcategories.Create(subcateg);
            uow.Save();
        }

        public void deleteSubcategory(int SubcategoryId)
        {
            DB_Subcategory subcategory = uow.Subcategories.FindById(SubcategoryId);
            if (subcategory != null)
                uow.Subcategories.Remove(subcategory);
            uow.Save();
        }
    }
}
