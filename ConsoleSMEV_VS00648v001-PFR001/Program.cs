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
            Paths.CreateFolder();
            X509Certificate2 certificate = Certificate.GetCert();

            ////Test send
            //string TestSendRequestFileName = "SendRequestRequestNoAttachTest.xml";
            //string file = Path.GetFullPath(Paths.Request() + TestSendRequestFileName);
            //string sigFile = Path.GetFullPath(Paths.Out() + "Sig_"+ TestSendRequestFileName);            
            //SignXml.Signed(file, certificate, sigFile);
            //Send.Go(sigFile);


            //Test get
            string TestGetRequestFileName = "GetResponseRequestTest.xml";
            string file = Path.GetFullPath(Paths.Request() + TestGetRequestFileName);
            string sigFile = Path.GetFullPath(Paths.Out() + "Sig_" + TestGetRequestFileName);
            SignXml.Signed(file, certificate, sigFile);
            Send.Go(sigFile);

            Console.WriteLine("end");

            Console.ReadKey();
        }
    }
}
