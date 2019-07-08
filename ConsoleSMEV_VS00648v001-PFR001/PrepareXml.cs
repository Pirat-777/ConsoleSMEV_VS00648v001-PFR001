using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSMEV_VS00648v001_PFR001
{
    class PrepareXml
    {
        private readonly X509Certificate2 certificate = Certificate.GetCert();
        public string Request(string XmlFileName)
        {
            string SigFileName = Paths.SigFileName(XmlFileName);
            SignXml.Signed(Paths.Request(XmlFileName), SigFileName, certificate);

            return SigFileName;
        }
    }
}
