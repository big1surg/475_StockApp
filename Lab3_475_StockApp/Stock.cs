using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab3_475_StockApp {
    /// <summary>
    /// class creates stock, and when stock reaches a certain threshold is sends our messages to its subscribers
    /// </summary>
    class Stock {
        string stockName; //name
        int initialValue; //stock price
        int maxChange; //stock price change
        int notifyWhenThisAmount; //value to measure to raise event
        int currentVal, count, x; //currentvalue of the stock, x is a variable for random number the stock goes up by, 
            //count is num of times thredhols is reached
        int numberChanges = 0; //keeps track of how many times the stock goes up
        //static AutoResetEvent autoEvent;
        public delegate void StockNotice(string stockName, int currentValue, int numberChanges); //delegate that sends information from
            //one object to another
        public event StockNotice StockEvent; //event to notify 
        public event StockNotice SaveStocks; //event to save stock to a file 
        Thread th; //thread

        /// <summary>
        /// default constructor
        /// </summary>
        public Stock() {
            stockName = "";
            initialValue = 0;
            maxChange = 5;
            notifyWhenThisAmount = 10;
            th = new Thread(activate);
            th.Start();
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="stockName">name of stock</param>
        /// <param name="initialValue">starting value</param>
        /// <param name="maxChange">amount the stock is permitted to go up by, 0-maxChange</param>
        /// <param name="notifyWhenThisAmount">notify broker when it has reached this threhold</param>
        public Stock(string stockName, int initialValue, int maxChange, int notifyWhenThisAmount) {
            this.stockName = stockName;
            this.initialValue = initialValue;
            this.maxChange = maxChange;
            this.notifyWhenThisAmount = notifyWhenThisAmount;
            //autoEvent = new AutoResetEvent(false);
            th = new Thread(activate); //the thread is created
            th.Start(); //thread starts
        }

        /// <summary>
        /// starts the thread increasing
        /// </summary>
        public void activate() {
            //autoEvent.Set();
            count = 0; //count is set to zero, increments each time threshold is reached
            currentVal = initialValue; //set current to initial
            Random r = new Random(); //create random 
            while (th.IsAlive && numberChanges<=99) { //while thread is alive and count !=5 (count = times threshold is reached)
                numberChanges++; //increase number of changes
                Thread.Sleep(500); //sleep for 500 milliseconds
                x = r.Next(0, maxChange); //set x equal to random value between 0 and maxChange
                currentVal += x; //currentvalue = currentval + the new random value
                if((currentVal - initialValue)>notifyWhenThisAmount) { //when difference is the threshold, enter this for loop
                    count++; 
                    //increase count
                    //invoke event method 
                     StockEvent(stockName, currentVal, numberChanges);
                     SaveStocks(stockName, initialValue, currentVal);
                }
            }
            th.Abort(); //kills thread
        }
       
        /// <summary>
        /// property for stock name
        /// </summary>
        public string StockName {
            get { return stockName; }
            set { stockName = value; }
        }

        /// <summary>
        /// propert for initialValue
        /// </summary>
        public int InitialValue {
            get { return initialValue; }
            set { initialValue = value; }
        }

        /// <summary>
        /// propert for maxchange
        /// </summary>
        public int MaxChange {
            get { return maxChange; }
            set { maxChange = value; }
        }

        /// <summary>
        /// property for currentvalue
        /// </summary>
        public int CurrentValue {
            get { return currentVal; }
            set { currentVal = value; }
        }

        /// <summary>
        /// property for number of changes
        /// </summary>
        public int NumberChanges {
            get { return numberChanges; }
        }

        /// <summary>
        /// property for notifying when threshold reached
        /// </summary>
        public int NotifyWhenThisAmount {
            get { return notifyWhenThisAmount; }
            set { notifyWhenThisAmount = value; }
        }

    }
}
