using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab3_475_StockApp {
    /// <summary>
    /// This class is for StockBroker
    /// -it creates a stock broker who watches stock
    /// </summary>
    class StockBroker {

        string brokerName; //name of broker
        List<Stock> stockList = new List<Stock>(); //list of stock that the broker watches
        //directory for the file
        private const string dir = @"C:\Users\sinci\Documents\Visual Studio 2015\Projects\Lab3_475_StockApp\Lab3_475_StockApp\";
        private const string path = dir + "TextFile1.txt";
        //thread synchronization
        EventWaitHandle waitHandle = new EventWaitHandle(true, EventResetMode.AutoReset, "SHARED_BY_ALL_PROCESSES");
        private FileStream myFileStream;

        /// <summary>
        /// default constructor
        /// </summary>
        public StockBroker(){ brokerName = "";}

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="brokerName">brokers name</param>
        public StockBroker(string brokerName) { this.brokerName = brokerName;}

        /// <summary>
        /// method to add stock to list of stock that the broker watches
        /// also contains two event handlers
        /// one event notifies when the stock has reached a certain threshold
        /// one event saves to a textfile when the stock has reached a certain threshold
        /// </summary>
        /// <param name="stock">the stock that the broker is watching</param>
        public void AddStock(Stock stock) {
            stockList.Add(stock);
            lock (stock) {
                stock.StockEvent += new Stock.StockNotice(this.Notify); //the subscriber, subscribes to the delegate on stock
                stock.SaveStocks += new Stock.StockNotice(this.SaveStock); //the subscriber, subscribes to the delegate on the stock
            }
        }

        /// <summary>
        /// event handler for the event when the threshold is reached
        /// </summary>
        /// <param name="name">stock name</param>
        /// <param name="val">stock current value</param>
        /// <param name="change">amount of times the stock changed before reaching the threshold</param>
        public void Notify(string name, int val, int change) {
            waitHandle.WaitOne(); //thread is locked
            //display stock name, current value, and number of changes
            Console.WriteLine("{0} {1} {2} {3}", brokerName.PadRight(20), name.PadRight(20), 
                val.ToString().PadRight(20), change.ToString().PadRight(20));
            waitHandle.Set(); //released
        }

        /// <summary>
        /// event handler for the event when the threshold is reached
        /// </summary>
        /// <param name="name">stock name</param>
        /// <param name="iniVal">initial value of stock</param>
        /// <param name="currVal">current value of stock</param>
        public void SaveStock(string name, int iniVal, int currVal) {
            try { //lock
                while (true) {
                    try {
                        myFileStream = new FileStream(path, FileMode.Append); //open file stream
                        break;
                    } catch {
                        Thread.Sleep(20);
                    }
                }
                StreamWriter outputFile = new StreamWriter(myFileStream);
                //date and time, name of stock, initial value of stock, and current value of stock
                outputFile.WriteLine("{0} {1} {2} {3}", System.DateTime.Now.ToString().PadRight(30),
                        name.PadRight(20), iniVal.ToString().PadRight(10), currVal.ToString().PadRight(10));
                //}
                outputFile.Close();
                myFileStream.Close();
                //myFileStream.Dispose();
            } finally {
                if (myFileStream != null) {
                    myFileStream.Dispose();
                }
            }
        }
    }
}
