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
            return SigXml("request", XmlFileName);
        }
        public string Ack(string XmlFileName)
        {            
            return SigXml("ack", XmlFileName);
        }

        private string SigXml(string action, string XmlFileName)
        {
            string SigFileName = Paths.SigFileName(XmlFileName);
            SignXml.Signed(GetActionXmlPath(action, XmlFileName), SigFileName, certificate);

            return SigFileName;
        }

        private string GetActionXmlPath(string action, string XmlFileName)
        {
            switch (action)
            {
                case "request":
                    return Paths.Request(XmlFileName);                    
                case "ack":
                    return Paths.Ack(XmlFileName);
                case "response":
                    return "";                    
                default:
                    return "";
            }
        }
    }
}
