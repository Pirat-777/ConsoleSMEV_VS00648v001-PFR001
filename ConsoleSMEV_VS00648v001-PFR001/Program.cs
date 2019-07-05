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

            string fileName = "SendRequestRequestNoAttach.xml";
            string file = Path.GetFullPath(Paths.Request() + fileName);
            string sigFile = Path.GetFullPath(Paths.Out() + "Sig_"+fileName);


            SignXml.Signed(file, certificate, sigFile);
        }
    }
}
