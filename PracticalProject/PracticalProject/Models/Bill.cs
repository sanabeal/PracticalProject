using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalProject.Models
{
    public class Bill
    {
        public int BillMasterID { get; set; }
        public string BillDate { get; set; }
        public string CustomerName { get; set; }
        public string ContactNo { get; set; }



        public int BillDetailsID { get; set; }
        public int ItemID { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal ItemQty { get; set; }
        public decimal TotalPrice { get; set; }
    }
}