using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class DB_Lot
    {
        [Key]
        [Required]
        public int LotId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Specification { get; set; }

        [Required]
        public int Bet { get; set; }
        public int? Step { get; set; }

        
        [Required]
        public DB_Category Category { get; set; }
        public int? SubcategoryId { get; set; }

        [Required]
        public int Duration { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        [Required]
        public DB_User Owner { get; set; }
        [MaxLength(200)]
        public string Winner { get; set; }
    }
}


