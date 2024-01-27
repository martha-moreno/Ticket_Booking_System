using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace _2Final
{
    public delegate void promotionalEvent(double price); //Defining a delegate

    public class Cruise
    {
        public static event promotionalEvent promotion;              //Linking event to delegate
        private static Int32 promotionLimit = 5;                     // Maximum number of promotions(price cuts) allowed
        private static Int32 countert = 0;                           // Countert  to track promotions(price cuts) made
        private double newTicketPrice = 20.00;                        // New ticket price variable
        private double ticketPrice = 100.00;                         // variable that holds cruise ticket price
        private ArrayList orderProcessingThread = new ArrayList();   //Array for new order threads

        public Int32 PromotionLimit { get; set; }
        public void CruiseMethod()
        {   

                while (countert < promotionLimit)                       //Condition to terminate cruise thread after a number of promotions(price cuts) have been made
                {
                    lock(this)
                   { 
                         PricingModel();                                 //sends pricing
                        if (newTicketPrice < ticketPrice && promotion != null) //checking that there is a lower price available vs previous and at least one subscriber to the event
                        {
                            promotionEvent();
                        }
                        ticketPrice = newTicketPrice;
                        OrderProcessing(Program.MultiCellBuffer.getOneCell());  //Process orders that are read from the multicellbuffer          
                    }                  
                } 
        }
/***************************************************************PRICING MODEL METHOD STARTS***********************************************************/

        public double PricingModel() // Defines ticket price 
        {
                Random random = new Random();
                newTicketPrice = random.Next(40, 200); // assigns a random number between 40 and 200 as new ticket price
                return newTicketPrice;
            
        }
/***************************************************************PRICING MODEL METHOD ENDS***********************************************************/

        public void promotionEvent()
        {
            lock (this)
            {
                countert++;
                promotion(newTicketPrice);               //notifies about the pricedrop to subscribers   
            }
        }
/***************************************************************ORDER PROCESSING METHODS START***********************************************************/

        public void OrderProcessing(OrderClass order)           //method that allows to process orders
        {
               if (order != null)                                  //If a new order is available
               {
                    order = Program.MultiCellBuffer.getOneCell(); //reads order from MultiCellBuffer and assigns value to order 
                    //Thread.Sleep(1000);  
                    if (order.CardNo >= 5000 && order.CardNo <= 7000) //Checking validity of credit card number received
                    {
                        lock (this)
                        {
                            //Console.WriteLine("ORDER from {0} ACCEPTED: Credit card has been validated ", Thread.CurrentThread.Name);
                            //Creating order threads
                             Thread thread = new Thread(() => processOrder(order));
                             orderProcessingThread.Add(thread);
                             thread.Start();

                        }

                    }
            }
            
        }

        public void processOrder(OrderClass order)
        {
            lock (this)
            {       
                double tax = 1.08;
                double locationCharge = 5;
                double totalCharge = order.Quantity * order.UnitPrice * tax + locationCharge;//Calculates total charge
                Console.WriteLine("PRINTING ORDER->: SenderId:{0}, CardNo:{1}, Quantity:{2}, UnitPrice:${3}, Total Charge:{4}", order.SenderId, order.CardNo, order.Quantity, order.UnitPrice, totalCharge );
                Thread.Sleep(1000);
            }
        }
/***************************************************************ORDER PROCESSING METHODS END***********************************************************/

    }
}

