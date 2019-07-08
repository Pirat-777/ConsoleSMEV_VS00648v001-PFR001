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
        public void Go(string fileName)
        {
            string MessageId = GetMessageID(fileName);
            if(MessageId != "")
            {
                GetAckRequest(MessageId);
            }
            else
            {
                Console.WriteLine("Ack: нет MessageID");
            }
            
        }

        private void GetAckRequest(string MessageID)
        {
            string AckShablon = Parametrs.Get("XmlSablon:AckShablon").First().Value;

            string pathAckShablon = Paths.Ack(AckShablon);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(pathAckShablon);

            if (xmlDoc.GetElementsByTagName("AckTargetMessage", "*")[0] != null)
            {
                xmlDoc.GetElementsByTagName("AckTargetMessage", "*")[0].InnerText = MessageID;

                // Сохраняем документ в файл.                
                using (XmlTextWriter xmltw = new XmlTextWriter(pathAckShablon, new UTF8Encoding(false)))
                {
                    xmlDoc.WriteTo(xmltw);
                }

                Send.Go(out _, out _, new PrepareXml().Ack(AckShablon));

                Console.WriteLine("Ack + MessageID: выполнено.");
            }
            else
            {
                Console.WriteLine("Ack: Тег AckTargetMessage в xml не найден");
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
