﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDxGridControl.Models
{
    public class LoadTruckMatch
    {
        public int Id { get; set; }
        public string SaleOrderNo { get; set; }

        public string CustomerNo { get; set; }

        public string MixdesignNo { get; set; }

        public decimal Amount { get; set; }
        public int LoadNumber { get; set; }

        public string ShipToNo { get; set; }
    }
}
