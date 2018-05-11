using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using maestroSMS.Class;
namespace maestroSMS
{
    class Program
    {
        private static SerialPort mySerialPort = new SerialPort("COM4",115200);
        private static config _config = new config();
        private static sms _sms = new sms();
        static void Main(string[] args)
        {
           //DevMode();
           argsRun(args);
        }
        public static void DevMode() {
            Console.WriteLine(_sms.sendSMS(mySerialPort, "+66945153598", "This message for test"));
            Console.ReadLine();
        }
        public static void argsRun(string[] args) {
            string curFile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\temp.txt";
            if (File.Exists(curFile))
            {
                string readText = File.ReadAllText(curFile);
                mySerialPort.PortName = readText.Split(',')[0];
                mySerialPort.BaudRate = Int32.Parse(readText.Split(',')[1]);
            }
            //----------------help--------------
            if (args.Contains("-h") || args.Contains("help"))
            {
                printHelp();
            }
            //---------------allport------------
            else if (args.Contains("-sp") || args.Contains("serialport"))
            {
                _config.GetAllPort();
            }
            //---------------status-------------
            else if (args.Contains("-st") || args.Contains("status"))
            {
                _config.GetConfigNow(mySerialPort);
            }
            //----------------set---------------
            else if (args.Contains("-c") || args.Contains("config"))
            {
                for (int i = 0; i < args.Length; i++)
                {
                    var word = Convert.ToString(args[i]).Split('=');
                    //portName
                    if (word[0].Equals("-pn") || word[0].Equals("portName"))
                    {

                        mySerialPort.PortName = word[1];
                    }
                    //baudRate
                    else if (word[0].Equals("-br") || word[0].Equals("baudRate"))
                    {
                        mySerialPort.BaudRate = Int32.Parse(word[1]);
                    }
                }
                string createText = mySerialPort.PortName + "," + mySerialPort.BaudRate;
                File.WriteAllText(curFile, createText);
            }
            //---------------send----------------
            else if (args.Contains("-s") || args.Contains("send"))
            {
                string phoneNO = "";
                string Message = "";
                for (int i = 0; i < args.Length; i++)
                {
                    var word = Convert.ToString(args[i]).Split('=');
                    //phone
                    if (word[0].Equals("-p") || word[0].Equals("phone"))
                    {

                        phoneNO = word[1].Replace("\"", "");
                    }
                    //message
                    else if (word[0].Equals("-m") || word[0].Equals("message"))
                    {
                        Message = word[1].Replace("\"", "");
                    }
                }
                if (args.Length == 3)
                {
                    var res = _sms.sendSMS(mySerialPort, phoneNO, Message);
                    Console.WriteLine("");
                    Console.WriteLine(res);
                    Console.WriteLine("");
                }
                else
                {
                    Console.WriteLine("");
                    Console.WriteLine("Please check your arguments.");
                    Console.WriteLine("maestroSMS send phone=\"+66xxxxxxxxx\" message=\"Your message\"");
                    Console.WriteLine("");
                }
            }
            else
            {
                printHelp();
            }
        }
        public static void printHelp()
        {
            Console.WriteLine("");
            Console.WriteLine("maestroSMS <command>");
            Console.WriteLine("");
            Console.WriteLine("  help (-h) : Quick help on <command>");
            Console.WriteLine("  serialport (-sp) : Show all serial port for connecting.");
            Console.WriteLine("  status (-st) : Status of SerialPort for program now.");
            Console.WriteLine("  config (-c) : config value for SerialPort.");
            Console.WriteLine("       |- portName=<value> (-pn=value) : Set portName for SerialPort.");
            Console.WriteLine("       |- baudRate=<value> (-br=value) : Set baudRate for SerialPort.");
            Console.WriteLine("  send (-s) : Prefix for send message.");
            Console.WriteLine("       |- phone=<value> (-p=value) : Set phone NO for send message.");
            Console.WriteLine("       |- message=<value> (-m=value) :  Set message for send message.");
            Console.WriteLine("  ------------------------------------------------------------------");
            Console.WriteLine("");
        }
    }
}
