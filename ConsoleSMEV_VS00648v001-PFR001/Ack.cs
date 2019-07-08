using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ConsoleSMEV_VS00648v001_PFR001
{
    class Ack
    {
        public void Send(string fileName)
        {
            GetMessageID(fileName);

        }

        protected void GetAckRequest(string MessageID)
        {            
            string pathAckReqShablon = SignSmevRequest.INI.ReadINI("AskXmlShablon", "path");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(pathAckReqShablon);

            if (xmlDoc.GetElementsByTagName("AckTargetMessage", "*")[0] != null)
            {
                xmlDoc.GetElementsByTagName("AckTargetMessage", "*")[0].InnerText = MessageID;

                // Сохраняем документ в файл.
                string fileName = SignSmevRequest.INI.ReadINI("outxml", "path")
                                                + Path.GetFileNameWithoutExtension(pathAckReqShablon)
                                                + "_"
                                                + MessageID
                                                + "_"
                                                + DateTime.Now.ToString("yyyyMMddTHHmmssfff")
                                                + ".xml";
                using (XmlTextWriter xmltw = new XmlTextWriter(fileName, new UTF8Encoding(false)))
                {
                    xmlDoc.WriteTo(xmltw);
                }

                SigAckFiles(fileName);
                TZip.FilesToZip("MaskOutXml", "outxml", "Out");
                new SignSmevRequest().LogMessage("Ack + MessageID: выполнено.", 1, 1, 10);
            }
            else
            {
                new SignSmevRequest().LogMessage("Ack: Тег AckTargetMessage в xml не найден", 1, 1);
            }
        }

        private string GetMessageID(string fileName)
        {
            string XmlText = File.ReadAllText(fileName);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(XmlText);

            string MessageID = "";
            if (xmlDoc.GetElementsByTagName("MessageId", "*")[0] != null)
            {
                return xmlDoc.GetElementsByTagName("MessageId", "*")[0].InnerText;
            }
            else if (xmlDoc.GetElementsByTagName("MessageID", "*")[0] != null)
            {
                MessageID = xmlDoc.GetElementsByTagName("MessageID", "*")[0].InnerText;
            }
            return MessageID;
        }
    }
}
