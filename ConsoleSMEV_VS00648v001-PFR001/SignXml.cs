using CryptoPro.Sharpei.Xml;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;

namespace ConsoleSMEV_VS00648v001_PFR001
{
    static class SignXml
    {
        public static bool Signed(string file, X509Certificate2 certificate, string sigFileName = "", bool MessageID = true)
        {
            //-------------------------------------------
            // Создаем новый документ XML.
            XmlDocument doc = new XmlDocument
            {
                PreserveWhitespace = true
            };
            // Читаем документ из файла.
            XmlTextReader XmlTxtRead = new XmlTextReader(file);
            doc.Load(XmlTxtRead);
            XmlTxtRead.Close();

            // Создаём объект SmevSignedXml - наследник класса SignedXml с перегруженным GetIdElement
            // для корректной обработки атрибута wsu:Id. 
            _SignedXml _signedXml = new _SignedXml(doc)
            {
                // Задаём ключ подписи для документа SmevSignedXml.
                SigningKey = certificate.PrivateKey
            };
            //-------------------------------------------
            
            string IdMessageTypeSelector = "";
            if (doc.GetElementsByTagName("MessageTypeSelector", "*")[0] != null)
            {
                IdMessageTypeSelector = doc.GetElementsByTagName("MessageTypeSelector", "*")[0].Attributes["Id"].Value;
            }

            if (doc.GetElementsByTagName("AckTargetMessage", "*")[0] != null)
            {
                IdMessageTypeSelector = doc.GetElementsByTagName("AckTargetMessage", "*")[0].Attributes["Id"].Value;
            }

            if (doc.GetElementsByTagName("SenderProvidedResponseData", "*")[0] != null)
            {
                IdMessageTypeSelector = doc.GetElementsByTagName("SenderProvidedResponseData", "*")[0].Attributes["Id"].Value;
            }

            if (doc.GetElementsByTagName("Timestamp", "*")[0] != null)
            {
                if (doc.GetElementsByTagName("Timestamp", "*")[0].Attributes["Id"] != null)
                {
                    IdMessageTypeSelector = doc.GetElementsByTagName("Timestamp", "*")[0].Attributes["Id"].Value;
                }
            }

            if (doc.GetElementsByTagName("SenderProvidedRequestData", "*")[0] != null)
            {
                IdMessageTypeSelector = doc.GetElementsByTagName("SenderProvidedRequestData", "*")[0].Attributes["Id"].Value;
            }
            
            var sign = certificate.PrivateKey;
            // Создаем ссылку на подписываемый узел XML. В данном примере и в методических
            // рекомендациях СМЭВ подписываемый узел soapenv:Body помечен идентификатором "body".
            Reference reference = new Reference
            {
                Uri = "#" + IdMessageTypeSelector
            };

            // Задаём алгоритм хэширования подписываемого узла - ГОСТ Р 34.11-94. Необходимо
            // использовать устаревший идентификатор данного алгоритма, т.к. именно такой
            // идентификатор используется в СМЭВ.
            if (certificate.GetKeyAlgorithm() != "1.2.643.7.1.1.1.1")
            {
                reference.DigestMethod = CryptoPro.Sharpei.Xml.CPSignedXml.XmlDsigGost3411UrlObsolete;//ГОСТ Р 34.11-94 
            }
            else
            {
                reference.DigestMethod = CryptoPro.Sharpei.Xml.CPSignedXml.XmlDsigGost3411_2012_256Url;//ГОСТ Р 34.11-2012 256бит
            }
                                   
            // Добавляем преобразование для приведения подписываемого узла к каноническому виду
            // по алгоритму http://www.w3.org/2001/10/xml-exc-c14n# в соответствии с методическими
            // рекомендациями СМЭВ.
            XmlDsigExcC14NTransform c14 = new XmlDsigExcC14NTransform();
            reference.AddTransform(c14);
            _signedXml.AddReference(reference);

            // Добавляем преобразование для приведения подписываемого узла к каноническому виду
            // по алгоритму urn://smev-gov-ru/xmldsig/transform в соответствии с методическими
            // рекомендациями СМЭВ.
            XmlDsigSmevTransform dsig = new XmlDsigSmevTransform();
            reference.AddTransform(dsig);
            _signedXml.AddReference(reference);

            // Задаём преобразование для приведения узла ds:SignedInfo к каноническому виду
            // по алгоритму http://www.w3.org/2001/10/xml-exc-c14n# в соответствии с методическими
            // рекомендациями СМЭВ.
            _signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;

            // Задаём алгоритм подписи - ГОСТ Р 34.10-2001. Необходимо использовать устаревший
            // идентификатор данного алгоритма, т.к. именно такой идентификатор используется в
            // СМЭВ.
            if (certificate.GetKeyAlgorithm() != "1.2.643.7.1.1.1.1")
            {
                _signedXml.SignedInfo.SignatureMethod = CryptoPro.Sharpei.Xml.CPSignedXml.XmlDsigGost3410UrlObsolete;//ГОСТ Р 34.10-2001
            }
            else
            {
                _signedXml.SignedInfo.SignatureMethod = CryptoPro.Sharpei.Xml.CPSignedXml.XmlDsigGost3410_2012_256Url;//ГОСТ Р 34.10-2012 256бит
            }

            // Create a new KeyInfo object.
            KeyInfo keyInfo = new KeyInfo();

            // Load the certificate into a KeyInfoX509Data object
            // and add it to the KeyInfo object.
            keyInfo.AddClause(new KeyInfoX509Data(certificate));

            // Add the KeyInfo object to the SignedXml object.
            _signedXml.KeyInfo = keyInfo;

            if (doc.GetElementsByTagName("Timestamp", "*")[0] != null)
            {
                //Добавляем время
                doc.GetElementsByTagName("Timestamp", "*")[0].InnerText = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffzzzzz");
            }

            if (MessageID)
            {
                if (doc.GetElementsByTagName("MessageID", "*")[0] != null)
                {
                    //Добавляем UUID
                    doc.GetElementsByTagName("MessageID", "*")[0].InnerText = GuidGenerator.GenerateTimeBasedGuid().ToString();
                }
            }

            // Вычисляем подпись.
            _signedXml.ComputeSignature("ds");
            // Получаем представление подписи в виде XML.
            XmlElement xmlDigitalSignature = _signedXml.GetXml("ds");
            
            doc.GetElementsByTagName("CallerInformationSystemSignature","*")[0].PrependChild(
                doc.ImportNode(xmlDigitalSignature, true));
         
            if (sigFileName == "")
                sigFileName = Paths.App() + "\\out\\sig_" + Path.GetFileName(file);

            XmlTextWriter xmltw = new XmlTextWriter(sigFileName, new UTF8Encoding(false));
            doc.WriteTo(xmltw);
            xmltw.Close();
            
            return true;
        }
    }

}
