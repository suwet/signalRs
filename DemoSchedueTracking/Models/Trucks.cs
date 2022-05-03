using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDxGridControl.Models
{
    public class Trucks
    {
        public int Id { get; set; }
        public string TruckCode { get; set; }
        public string PlantCode { get; set; }
        public string TimeToSite { get; set; }
        public double TruckSize { get; set; }

        public int Load_Status { get; set; }
    }
}
