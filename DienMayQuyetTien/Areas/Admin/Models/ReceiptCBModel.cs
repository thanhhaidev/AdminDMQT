using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DienMayQuyetTien.Areas.Admin.Models
{
    public class ReceiptCBModel
    {
        public string BillCode { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Shipper { get; set; }
        public string Note { get; set; }
        public Nullable<int> GrandTotal { get; set; }

        public List<CashBillDetail> cashBillDetail { get; set; }
    }
}