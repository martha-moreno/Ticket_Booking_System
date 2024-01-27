using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
namespace _2Final
{

    //Project 2 Description: New ticket block booking system that involves ticket agents and cruises. The system consist of N=3 ticket agents and 1 cruise.
    //The Ticket Agents can buy quantity (block) of tickets from the cruises with lower prices.

    public class Program
    {
        private static Int32 N = 3;  // number of Ticket Agents
        public static MultiCellBuffer MultiCellBuffer = new MultiCellBuffer(); //object of shared resource
        private static Thread[] agentThreads = new Thread[N];


        static void Main(string[] args)
        {

            Cruise cruise = new Cruise();//creating an instance of Cruise
            Thread cruiseThread = new Thread(cruise.CruiseMethod);//Creating a cruise thread that will execute CruiseMethod at start
            cruiseThread.Name = "Disney Cruise Line"; //Naming the cruise thread
            cruiseThread.Start(); //starting the cruise thread

            TicketAgent agentThread = new TicketAgent(); //creating an instance of TicketAgent
            Cruise.promotion += new promotionalEvent(agentThread.cruiseTicketOnSale); //subscribing to event

            for (int i = 0; i < N; ++i)
            {
                agentThreads[i] = new Thread(agentThread.TicketAgentMethod); //Creating a ticket agent thread that will execute TicketAgentMethod at start
                agentThreads[i].Name = "Ticket Agent" + i; //Naming the ticket agent thread
                agentThreads[i].Start(); //starting ticket agents thread
                Thread.Sleep(1000);
            }
            //Waiting for threads to finish executing
            cruiseThread.Join();
            for (int i = 0; i < N; ++i)
            {
                agentThreads[i].Join();
            }

            Console.WriteLine("Cruise Booking Application has ENDED");
        }
    }
}


