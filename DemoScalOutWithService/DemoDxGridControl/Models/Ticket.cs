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
        public int Load_Status { get; set; }
        public string LoadNo { get; set; }

        public string ShipToNo { get; set; }
        public string MixDesignNo { get; set; }

        public string AroundStatus { get; set; }

        public string ShipToSlip { get; set; }

        public string TruckCode { get; set; }

        public string DriverName { get; set; }

        public string ReturnTruckToPlantCode { get; set; }
    }
}
