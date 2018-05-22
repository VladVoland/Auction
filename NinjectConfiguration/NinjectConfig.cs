using BLL;
using DAL;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjectConfiguration
{
    public class NinjectConfig : NinjectModule
    {
        public override void Load()
        {
            Bind<DAL.IUnitOfWork>().To<DAL.UnitOfWork>();
            Bind<BLL.ICategory_Operations>().To<BLL.Category_Operations>();
            Bind<BLL.ISubcategory_Operations>().To<BLL.Subcategory_Operations>();
            Bind<BLL.ILot_Operations>().To<BLL.Lot_Operations>();
            Bind<BLL.IUser_Operations>().To<BLL.User_Operations>();
            Bind<DAL.UnitOfWork>().ToSelf();
            Bind<Category_Operations>().ToSelf();
            Bind<Subcategory_Operations>().ToSelf();
            Bind<Lot_Operations>().ToSelf();
            Bind<User_Operations>().ToSelf();
        }
    }
}
