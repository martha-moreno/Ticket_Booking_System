using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace _2Final
{
    public class MultiCellBuffer //Class that is used for the communication between the ticket agents and the cruise
    {
        private static Int32 n = 2;                 //number of data cells in the buffer
        string[] multiCellBuffer = new string[n];   //Array that holds orders in string format
        private static Int32 initialCount = 2;      //variable for keeping track of available spaces according to semaphore
        private static Int32 maxCount = 2;           //variable that determines max number of spaces according to semaphore

        private static Semaphore writeSemaphoreObject = new Semaphore(initialCount, maxCount); //Semaphore initialization with maxCount of 2 to define the max number of threads that can enter
        private static Semaphore readSemaphoreObject = new Semaphore(1, 1); //Semaphore initialization for allowing cruise to read

        Int32 index1 = 0;
        Int32 index = 0;
        Int32 spacesAvailable = 0;
/**********************************************SETONECELL METHOD STARTS***********************************************************************/
        public void setOneCell(string order)
        {
            writeSemaphoreObject.WaitOne();
            //Console.WriteLine("{0} entered write", Thread.CurrentThread.Name);
            lock (this)
            {
                while (spacesAvailable == n) //while all cells are occupied (value of zero)           
                {
                   // Console.WriteLine("{0} is waiting to write an order to the multicellbuffer", Thread.CurrentThread.Name);
                    Monitor.Wait(this);

                }
                    multiCellBuffer[index] = order;  //assign order value to element of the array that is being written
                    index = (index + 1) % n;        //calculate the next index in the array
                   // Console.WriteLine("{0} is writing an order", Thread.CurrentThread.Name);
                    spacesAvailable++;
                  // Console.WriteLine("{0} is leaving write", Thread.CurrentThread.Name);
                    writeSemaphoreObject.Release();
                    Monitor.PulseAll(this); //Notify other threads
            }
            
        }
        /**********************************************SETONECELL METHOD ENDS***********************************************************************/
        
        /**********************************************GETONECELL METHOD STARTS***********************************************************************/
        public OrderClass getOneCell()
        { 
            readSemaphoreObject.WaitOne();
          //  Console.WriteLine("{0} entered read", Thread.CurrentThread.Name);
            lock (this)
            { 
                string order;
                while (spacesAvailable == 0)
                {
                    //Console.WriteLine("{0} is waiting to read an order", Thread.CurrentThread.Name);
                    Monitor.Wait(this);
                }
                order = multiCellBuffer[index1]; // array order element is assign to string variable
                index1 = (index + 1) % n;
                spacesAvailable--;
                //Console.WriteLine("{0} is reading order", Thread.CurrentThread.Name);
                readSemaphoreObject.Release();
                //Console.WriteLine("{0} Leaving Reading", Thread.CurrentThread.Name);
                Monitor.PulseAll(this);

                //Decoding order from string to orderClass
                OrderClass orderforprocessing = new OrderClass(); //an orderclass object is created
                string[] orderDetails = order.Split(','); //split received order string by comma to get individual data
                orderforprocessing.SenderId = orderDetails[0];
                orderforprocessing.CardNo = Int32.Parse(orderDetails[1]);
                orderforprocessing.Quantity = Int32.Parse(orderDetails[2]);
                orderforprocessing.UnitPrice = double.Parse(orderDetails[3]);
                return orderforprocessing;
            }
        }
        /**********************************************GETONECELL METHOD ENDS***********************************************************************/
    }
}
