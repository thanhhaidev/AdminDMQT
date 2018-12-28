using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DienMayQuyetTien.Areas.Admin.Models
{
    public class ReceiptIBModel
    {
        public int ID { get; set; }
        public string BillCode { get; set; }
        public int CustomerID { get; set; }
        public System.DateTime Date { get; set; }
        public string Shipper { get; set; }
        public string Note { get; set; }
        public string Method { get; set; }
        public int Period { get; set; }
        public int GrandTotal { get; set; }
        public int Taken { get; set; }
        public int Remain { get; set; }
        public List<InstallmentBillDetail> installmentBillDetail { get; set; }
    }
}