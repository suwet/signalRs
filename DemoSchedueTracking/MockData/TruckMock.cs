using DemoDxGridControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDxGridControl.MockData
{
    public class TruckMock
    {
        public static List<Trucks> GetTrucks()
        {
            var tk = new List<Trucks>()
            {
                new Trucks()
                {
                    Id=1,
                    PlantCode="S101",
                    TimeToSite="56253",
                     TruckCode = "FS199",
                      TruckSize = 6.0

                },
                new Trucks()
                {
                    Id=2,
                    PlantCode="S101",
                    TimeToSite="1190",
                     TruckCode = "FS199",
                      TruckSize = 6.0

                },
                new Trucks()
                {
                    Id=3,
                    PlantCode="S101",
                    TimeToSite="1190",
                     TruckCode = "FS199",
                      TruckSize = 6.0

                },
                new Trucks()
                {
                    Id=4,
                    PlantCode="S101",
                    TimeToSite="1190",
                     TruckCode = "FS199",
                      TruckSize = 6.0

                },
            };

            return tk;
        }
    }
}
