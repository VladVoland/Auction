using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface ILot_Operations
    {
        IUnitOfWork uow { get; set; }
        void SaveLot(Lot _lot);
        void deleteLot(int LotId);
        List<Lot> GetUnconfirmedLots();
        List<Lot> GetСonfirmedLots();
        bool Change(string name, string specification, int LotId);
        List<Lot> GetEndedLots();
        bool ChangeBet(int bet, int winnerId, int LotId);
        List<Lot> GetBySearch(string _category, string _subcategory, string keyword);
        void Confirm(int LotId);
    }
}
