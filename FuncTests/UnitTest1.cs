using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using BLL;
using DAL;
using Moq;
using AutoMapper;
using DAL.Entities;

namespace FuncTests
{
    [TestClass]
    public class UnitTest1
    {
        private Category_Operations CatgO;
        private Subcategory_Operations SubcO;
        private User_Operations UserO;
        private Lot_Operations LotO;
        Mock<IUnitOfWork> mockContainer;
        Mock<AuctionDB> mockModel;
        Mock<dbContextRepository<DB_Category>> mockCategories;
        Mock<dbContextRepository<DB_Subcategory>> mockSubcategories;
        Mock<dbContextRepository<DB_User>> mockUsers;
        Mock<dbContextRepository<DB_Lot>> mockLots;


        
    }
}
