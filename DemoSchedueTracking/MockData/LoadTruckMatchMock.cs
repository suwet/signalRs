using DemoDxGridControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDxGridControl.MockData
{
    public class LoadTruckMatchMock
    {
        public static List<Loads> GetEmptyLoadTruckMatches()
        {
            var ltm = new List<Loads>()
            {
                
            };
            return ltm;
        }

        public static List<Loads> GetLoadTruckMatches()
        {
            var ltm = new List<Loads>()
            {
                new Loads()
                {
                     Id=1,
                     Amount=6,
                     LoadNumber=1,
                     MixdesignNo="990620835",
                     SaleOrderNo = "S03P000-000000117",
                     CustomerNo="ST_001",
                     ShipToNo = "SH_Name193"

                },
                new Loads()
                {
                     Id=2,
                     Amount=6,
                     LoadNumber=2,
                     MixdesignNo="990620835",
                     SaleOrderNo = "S03P000-000000117",
                     CustomerNo="ST_001",
                     ShipToNo = "SH_Name193"
                },
                 new Loads()
                {
                     Id=3,
                     Amount=6,
                     LoadNumber=3,
                     MixdesignNo="990620835",
                     SaleOrderNo = "S03P000-000000117",
                     CustomerNo="ST_001",
                     ShipToNo = "SH_Name193"
                },

                  new Loads()
                {
                     Id=4,
                     Amount=6,
                     LoadNumber=4,
                     MixdesignNo="990620835",
                     SaleOrderNo = "S03P000-000000117",
                     CustomerNo="ST_001",
                     ShipToNo = "SH_Name193"
                },

                   new Loads()
                {
                     Id=5,
                     Amount=6,
                     LoadNumber=5,
                     MixdesignNo="990620835",
                     SaleOrderNo = "S03P000-000000117",
                     CustomerNo="ST_001",
                     ShipToNo = "SH_Name193"
                },

                    new Loads()
                {
                     Id=6,
                     Amount=6,
                     LoadNumber=6,
                     MixdesignNo="990620835",
                     SaleOrderNo = "S03P000-000000117",
                     CustomerNo="ST_001",
                     ShipToNo = "SH_Name193"
                },
            };
            return ltm;
        }
    }
}
