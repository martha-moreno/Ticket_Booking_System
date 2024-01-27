using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2Final
{
    //Class that contains private data members (senderId, cardNo, quantity, unitPrice)
    public class OrderClass
    {
        private string senderId;
        private Int32 cardNo;
        private Int32 quantity;
        private double unitPrice;

        public string SenderId { get; set; }
        public Int32 CardNo { get; set; }
        public Int32 Quantity { get; set; }
        public double UnitPrice { get; set; }
    }
}
