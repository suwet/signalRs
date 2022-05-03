using DemoDxGridControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDxGridControl.MockData
{
    public class TicketMock
    {
        static Ticket ticket = new Ticket()
        {
            SaleOrderNo = "S03P000-000000117",
            TruckCode = "FS199",
            CustomerNo = "Cus_Name138",
            Amount = 6,
            Id = 1,
            LoadNo = "1",
            DriverName = "DVFWS",
            Load_Status = 1,
            ShipToNo = "SH_Name193",
            AroundStatus = "1/6",
            MixDesignNo = "990620835",
            ReturnTruckToPlantCode = "S101:S101",
            ShipToSlip = "101220170001"
        };

        static List<Ticket> Tickets = new List<Ticket>()
        {
            ticket
        };


        public static List<Ticket> GetEmptyTickets()
        {
            var tk = new List<Ticket>()
            {
                
            };
            return tk;
        }

        public static List<Ticket> GetTickets()
        {
            //var tk = new List<Ticket>()
            //{
            //    new Ticket()
            //    {
            //        SaleOrderNo="S03P000-000000117",
            //        TruckCode="FS199",
            //        CustomerNo="Cus_Name138",
            //        Amount = 6,
            //        Id=1,
            //        LoadNo="1",
            //        DriverName="DVFWS",
            //        Load_Status=3,
            //        ShipToNo="SH_Name193",
            //        AroundStatus="1/6",
            //        MixDesignNo="990620835",
            //        ReturnTruckToPlantCode="S101:S101",
            //        ShipToSlip = "101220170001"
            //    }
            //};
            return Tickets;
        }

    }
}
