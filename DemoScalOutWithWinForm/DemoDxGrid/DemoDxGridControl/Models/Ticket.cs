using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDxGridControl.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string SaleOrderNo { get; set; }
        public string CustomerNo { get; set; }
        public decimal Amount { get; set; }
        public int _Status { get; set; }
        public string LoadNo { get; set; }
    }
}
