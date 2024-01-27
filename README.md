#Project 2
#CSE Distributed Systems and Design - Ticket booking system in multithreading system

Architecture of a new ticket booking system in multithreading system
<img width="828" alt="Screenshot 2024-01-26 at 5 38 35 PM" src="https://github.com/martha-moreno/Ticket_Booking_System/assets/88118070/49985f1c-4893-4b95-9a72-b627d0a660d1">

Description:
An Operation Scenario of the ticket/cruise booking system is outlined as follows:
(1) A Cruise uses a pricing model to calculate dynamically the ticket price for the ticket agents. The
prices can go up and down from time to time. If the new price is lower than the previous price, it
emits a (promotional) event and calls the event handlers in the ticket agents (clients) that have
subscribed to the event.
(2) A TicketAgent evaluates the needs based on the new price and other factors, generates an
OrderObject (consisting of multiple values), and sends the order to the cruise through a
MultiCellBuffer.
(3) The TicketAgent sends the OrderObject to the promoting cruise through one of the free cells in
the MultiCellBuffer.
(4) The Cruise receives the OrderObject from the MultiCellBuffer.
(5) The Cruise creates a new thread, an OrderProcessingThread, to process the order;
(6) The OrderProcessingThread processes the order, e.g., checks the credit card number and the
maximum number allowed to purchase, etc., and calculates the total amount.
(7) The OrderProcessingThread sends a confirmation to the ticket agent and prints the order (on
screen).
