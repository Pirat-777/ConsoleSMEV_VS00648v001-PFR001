using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSMEV_VS00648v001_PFR001
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Paths.CreateFolder();
                                
                Send.Go(new PrepareXml().Request("SendRequestRequestNoAttachTest.xml"));
                Send.Go(new PrepareXml().Request("GetResponseRequestTest.xml"));
            }
            catch (Exception ex)
            {
                Console.WriteLine("err: {0}",ex.Message);
            }            

            Console.WriteLine("end");

            Console.ReadKey();
        }
    }
}
