using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class Lot
    {
        public Lot() { }
        public int LotId { get; set; }
        public string Name { get; set; }
        public string Specification { get; set; }
        public int Bet { get; set; }
        public int Step { get; set; }
        public string Category { get; set; }
        public string Subcategory { get; set; }
        public int SubcategoryId { get; set; }
        public int Duration { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Owner { get; set; }
        public string OwnerInfo { get; set; }
        public string Winner { get; set; }
    }
}
