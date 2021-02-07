using System;
using System.Dynamic;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;
using System.Net;

namespace DemoCollection
{
    class Program
    {
        static void Main(string[] args)
        {
            //String Address = Console.ReadLine();
            //IPAddress IP = IPAddress.Parse(Address);
            //Console.WriteLine("ip : {0}", IP.ToString());
            //IPAddress[] IP = Dns.GetHostAddresses("www.naver.com");
            //foreach(IPAddress HostIP in IP)
            //{
            //    Console.WriteLine("{0} ", HostIP);
            //}



            //IPHostEntry HostInfo = Dns.GetHostEntry("www.naver.com");

            //foreach (IPAddress ip in HostInfo.AddressList)
            //{
            //    Console.WriteLine("{0}", ip);
            //}

            //Console.WriteLine("{0}", HostInfo.HostName);

            IPAddress IPInfo = IPAddress.Parse("127.0.0.1");
            int Port = 13;
            IPEndPoint EndPointInfo = new IPEndPoint(IPInfo, Port);
            Console.WriteLine("ip: {0}    port: {1}", EndPointInfo.Address, EndPointInfo.Port);
            Console.WriteLine(EndPointInfo.ToString());
            Console.ReadKey();
        }
    }
}

