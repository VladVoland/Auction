using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System.Collections.Generic;
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

        [Test]
        public void TestGetCategories()
        {
            BLL_AutoMapper.Initialize();
            ResetData();
            List<DB_Category> categories = new List<DB_Category>();
            DB_Category catg1 = new DB_Category();
            DB_Category catg2 = new DB_Category();
            categories.Add(catg1);
            categories.Add(catg2);

            mockContainer.Setup(a => a.Categories).Returns(mockCategories.Object);
            mockCategories.Setup(a => a.Get()).Returns(categories);
            List<Category> result = new List<Category>();
            result = Mapper.Map<List<DB_Category>, List<Category>>(categories);

            NUnit.Framework.Assert.AreEqual(result.Capacity, CatgO.GetCategories().Capacity);
        }

        [Test]
        public void TestGetSubcategories()
        {
            ResetData();
            List<DB_Subcategory> subcategories = new List<DB_Subcategory>();
            DB_Subcategory subcatg1 = new DB_Subcategory();
            DB_Subcategory subcatg2 = new DB_Subcategory();
            subcategories.Add(subcatg1);
            subcategories.Add(subcatg2);

            mockContainer.Setup(a => a.Subcategories).Returns(mockSubcategories.Object);
            mockSubcategories.Setup(a => a.GetWithInclude(s => s.Category)).Returns(subcategories);
            List<Subcategory> result = new List<Subcategory>();
            result = Mapper.Map<List<DB_Subcategory>, List<Subcategory>>(subcategories);

            NUnit.Framework.Assert.AreEqual(result.Capacity, SubcO.GetSubcategories().Capacity);
        }

        [Test]
        public void TestGetUnconfirmedLots()
        {
            ResetData();
            List<DB_Lot> lots = new List<DB_Lot>();
            DB_Lot lot1 = new DB_Lot();
            DB_Lot lot2 = new DB_Lot();
            lots.Add(lot1);
            lots.Add(lot2);
            DB_Subcategory subc = new DB_Subcategory();

            mockContainer.Setup(a => a.Lots).Returns(mockLots.Object);
            mockLots.Setup(a => a.GetWithInclude(l => (l.Category), l => (l.Owner))).Returns(lots);
            mockSubcategories.Setup(a => a.FindById(0)).Returns(subc);
            List<Lot> result = new List<Lot>();
            result = Mapper.Map<List<DB_Lot>, List<Lot>>(lots);

            NUnit.Framework.Assert.AreEqual(result.Capacity, LotO.GetUnconfirmedLots().Capacity);
        }

        [Test]
        public void TestGetСonfirmedLots()
        {
            ResetData();
            List<DB_Lot> lots = new List<DB_Lot>();
            DB_Lot lot1 = new DB_Lot();
            DB_Lot lot2 = new DB_Lot();
            lot1.StartDate = DateTime.Now;
            lot2.StartDate = DateTime.Now;
            lot1.EndDate = DateTime.Now.AddDays(1);
            lot2.EndDate = DateTime.Now.AddDays(1);
            lots.Add(lot1);
            lots.Add(lot2);

            mockContainer.Setup(a => a.Lots).Returns(mockLots.Object);
            mockLots.Setup(a => a.GetWithInclude(l => (l.Category), l => (l.Owner))).Returns(lots);
            List<Lot> result = new List<Lot>();
            result = Mapper.Map<List<DB_Lot>, List<Lot>>(lots);

            NUnit.Framework.Assert.AreEqual(result.Capacity, LotO.GetСonfirmedLots().Capacity);
        }

        [Test]
        public void TestGetEndedLots()
        {
            ResetData();
            List<DB_Lot> lots = new List<DB_Lot>();
            DB_Lot lot1 = new DB_Lot();
            DB_Lot lot2 = new DB_Lot();
            lot1.EndDate = DateTime.Now;
            lot2.EndDate = DateTime.Now;
            lots.Add(lot1);
            lots.Add(lot2);

            mockContainer.Setup(a => a.Lots).Returns(mockLots.Object);
            mockLots.Setup(a => a.GetWithInclude(l => (l.Category), l => (l.Owner))).Returns(lots);
            List<Lot> result = new List<Lot>();
            result = Mapper.Map<List<DB_Lot>, List<Lot>>(lots);

            NUnit.Framework.Assert.AreEqual(result.Capacity, LotO.GetEndedLots().Capacity);
        }

        public void ResetData()
        {
            mockContainer = new Mock<IUnitOfWork>();
            mockModel = new Mock<AuctionDB>();
            mockCategories = new Mock<dbContextRepository<DB_Category>>(mockModel.Object);
            mockSubcategories = new Mock<dbContextRepository<DB_Subcategory>>(mockModel.Object);
            mockLots = new Mock<dbContextRepository<DB_Lot>>(mockModel.Object);
            mockUsers = new Mock<dbContextRepository<DB_User>>(mockModel.Object);
            CatgO = new Category_Operations(mockContainer.Object);
            SubcO = new Subcategory_Operations(mockContainer.Object);
            UserO = new User_Operations(mockContainer.Object);
            LotO = new Lot_Operations(mockContainer.Object);
        }

    }
}
