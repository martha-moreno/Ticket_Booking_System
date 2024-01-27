using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace _2Final
{
    public class TicketAgent
    {
        private static double reducedPrice=100.00;
        private static Random random = new Random();
        private static Int32 r1 = random.Next(0, 5);
        private static Int32 countert = 0;

        public string SenderId { get; set; }
        public void TicketAgentMethod()
        {
            Cruise cruiseobject = new Cruise();
            Int32 Limit = cruiseobject.PromotionLimit;


            while (countert< 5) //maximum num of promotions(price-cuts) allowed
            {
                
                double cruisePrice = cruiseobject.PricingModel(); //get ticket pricing from Cruise class
                double priceDifference = cruisePrice - reducedPrice; //determine difference between previous ticket price and new 
                OrderClass order = new OrderClass();
                // Console.WriteLine("CRUISE QUOTE for {0}: Ticket Price -->${1}", Thread.CurrentThread.Name, cruisePrice);

                //Determine if Ticket Agent will buy a ticket and creates the order 
                if (priceDifference > 10)                                        //TicketAgent will place an order if price difference is > 10
                   {
                        lock (this)
                        {                       
                            order.SenderId = Thread.CurrentThread.Name;                  //assigns thread name to senderId
                            order.CardNo = random.Next(5000, 7000);                     //Generates a random credit card number between 5000 and 7000
                            order.Quantity = random.Next(1, 30);                        //Generates a random quantity of tickets to buy
                            order.UnitPrice = reducedPrice;                             //assigns reduced price to unit's price
                           
                            //Converts order to string separated by commas to send to multicellbuffer****ENCONDER****
                            string ordertostring = order.SenderId + "," + order.CardNo + "," + order.Quantity + "," + order.UnitPrice; //encondes order to string
                           Thread.Sleep(2000);
                            Program.MultiCellBuffer.setOneCell(ordertostring); //sends encoded order to MultiCellBuffer
                            
                          //  Console.WriteLine("{0} GENERATED A NEW ORDER! SenderId:{1}, CardNo:{2}, Quantity:{3}, UnitPrice:{4}", Thread.CurrentThread.Name, order.SenderId, order.CardNo, order.Quantity, order.UnitPrice);
                            
                        }              
                }
                
                
            }
          
        }
/************************************************************EVENT HANDLER**************************************************************************/
        public void cruiseTicketOnSale(double cruisePrice) //Event Handler
        {
            lock (this)
            {
                countert++;
                reducedPrice = cruisePrice;
                Console.WriteLine("EVENT PROMOTION NOTIFICATION #{0}: Cruise tickets are ON SALE as low as ${1}", countert, cruisePrice);
               Thread.Sleep(500);
            }
        }
     /************************************************************EVENT HANDLER**************************************************************************/

    }
}
