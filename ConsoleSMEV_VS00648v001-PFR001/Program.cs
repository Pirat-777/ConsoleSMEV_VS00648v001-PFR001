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
                                
                Send.Go(out _, out _, new PrepareXml().Request("SendRequestRequestNoAttachTest.xml"));

                Send.Go(out bool GetResult, out string GetPathInName, new PrepareXml().Request("GetResponseRequestTest.xml"));
                if (GetResult)
                {
                    new Ack().Go(GetPathInName);
                }
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
