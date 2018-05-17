using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using DAL;
using DAL.Entities;
using Ninject;

namespace BLL
{
    public class Lot_Operations : ILot_Operations
    {
        IKernel ninjectKernel;
        public IUnitOfWork uow { get; set; }
        public Lot_Operations()
        {
            ninjectKernel = new StandardKernel();
            ninjectKernel.Bind<IUnitOfWork>().To<UnitOfWork>();
            this.uow = ninjectKernel.Get<IUnitOfWork>();
        }
        public void SaveLot(Lot _lot)
        {
            DB_Lot lot = new DB_Lot();
            lot.Bet = _lot.Bet;
            lot.Duration = _lot.Duration;
            lot.Name = _lot.Name;
            lot.Specification = _lot.Specification;
            lot.Step = _lot.Step;
            lot.Winner = "No winner";

            double per = lot.Bet / 10;
            if (per > lot.Step)
                lot.Step = (int)per;

            IEnumerable<DB_Category> categories = uow.Categories.Get();
            foreach (DB_Category c in categories)
            {
                if (c.Name == _lot.Category)
                {
                    lot.Category = uow.Categories.FindById(c.CategoryId);
                }
            }
            if (!string.IsNullOrWhiteSpace(_lot.Subcategory))
            {
                IEnumerable<DB_Subcategory> subcategories = uow.Subcategories.Get();
                foreach (DB_Subcategory sc in subcategories)
                {
                    if (sc.Name == _lot.Subcategory)
                        lot.SubcategoryId = sc.SubcategoryId;
                }
            }
            else
                lot.SubcategoryId = null;
            DB_User owner = uow.Users.FindById(_lot.Owner);
            lot.Owner = owner;

            uow.Lots.Create(lot);
            uow.Save();
        }

        public void deleteLot(int LotId)
        {
            DB_Lot lot = uow.Lots.FindById(LotId);
            if (lot != null)
                uow.Lots.Remove(lot);
            uow.Save();
        }

        public List<Lot> GetUnconfirmedLots()
        {
            List<Lot> lots = new List<Lot>();
            IEnumerable<DB_Lot> dblots = uow.Lots.GetWithInclude(l => (l.Category), l => (l.Owner));
            foreach (DB_Lot lot in dblots)
            {
                if (lot.StartDate == new DateTime())
                {
                    Lot tempL = Mapper.Map<DB_Lot, Lot>(lot);
                    tempL.Owner = lot.Owner.UserId;
                    lots.Add(tempL);
                }
            }
            foreach(Lot lot in lots)
            {
                if (lot.SubcategoryId > 0)
                {
                    DB_Subcategory dbsubc = uow.Subcategories.FindById(lot.SubcategoryId);
                    lot.Subcategory = dbsubc.Name;
                }
                else lot.Subcategory = "";
            }
            return lots;
        }
        public List<Lot> GetСonfirmedLots()
        {
            List<Lot> lots = new List<Lot>();
            IEnumerable<DB_Lot> dblots = uow.Lots.GetWithInclude(l => (l.Category), l => (l.Owner));
            foreach (DB_Lot lot in dblots)
            {
                if (lot.StartDate != new DateTime(0001, 1, 1, 0, 0, 0) && lot.EndDate > DateTime.Now)
                {
                    Lot tempL = Mapper.Map<DB_Lot, Lot>(lot);
                    tempL.Owner = lot.Owner.UserId;
                    lots.Add(tempL);
                }
            }
            return lots;
        }

        public bool Change(string name, string specification, int LotId)
        {
            DB_Lot lot = uow.Lots.FindById(LotId);
            lot.Name = name;
            lot.Specification = specification;
            uow.Lots.Update(lot);
            uow.Save();
            return true;
        }

        public List<Lot> GetEndedLots()
        {
            List<Lot> lots = new List<Lot>();
            IEnumerable<DB_Lot> dblots = uow.Lots.GetWithInclude(l => (l.Category), l => (l.Owner));
            foreach (DB_Lot lot in dblots)
            {
                if (lot.EndDate <= DateTime.Now)
                {
                    Lot tempL = Mapper.Map<DB_Lot, Lot>(lot);
                    tempL.Owner = lot.Owner.UserId;
                    lots.Add(tempL);
                }
            }
            return lots;
        }

        public bool ChangeBet(int bet, int winnerId, int LotId)
        {
            DB_Lot lot = uow.Lots.FindById(LotId);
            if (lot.Bet + lot.Step < bet)
            {
                DB_User winner = uow.Users.FindById(winnerId);
                string winnerInfo = winner.Name + " " + winner.Surname + "; Phone number: " + winner.PhoneNumber;
                lot.Bet = bet;
                lot.Winner = winnerInfo;
                uow.Lots.Update(lot);
                uow.Save();
                return true;
            }
            return false;
        }


        public List<Lot> GetBySearch(string _category, string _subcategory, string keyword)
        {
            if (_category == null) _category = "";
            if (_subcategory == null) _subcategory = "";
            if (keyword == null) keyword = "";
            List<Lot> confirmedLots = GetСonfirmedLots();
            List<Lot> lots = new List<Lot>();
            foreach (Lot lot in confirmedLots)
            {
                if(_category != "" && lot.Category.Contains(_category))
                    lots.Add(lot);
                else if (_subcategory != "" && lot.Subcategory.Contains(_subcategory))
                    lots.Add(lot);
                else if(keyword != "")
                {
                    if (lot.Name.Contains(keyword)) lots.Add(lot);
                    else if (lot.Specification.Contains(keyword)) lots.Add(lot);
                }
            }
            return lots;
        }

        public void Confirm(int LotId)
        {
            UnitOfWork uow = new UnitOfWork();
            DB_Lot lot = uow.Lots.FindById(LotId);
            DateTime tempDT = DateTime.Now;
            lot.StartDate = tempDT;
            lot.EndDate = tempDT.AddDays(lot.Duration);
            uow.Lots.Update(lot);
            uow.Save();
        }
    }
}
