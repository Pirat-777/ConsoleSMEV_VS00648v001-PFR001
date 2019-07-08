using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace ConsoleSMEV_VS00648v001_PFR001
{
    class DataFromXml
    {
        //вернём массив данных из файла XML
        public Dictionary<string, string> Get(string file)
        {
            return GetDictionary(file);
        }

        protected XmlDocument ReadXml(string file)
        {
            string text = File.ReadAllText(file);            
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(text);
            return xmlDoc;
        }

        protected Dictionary<string, string> GetDictionary(string file)
        {
            XmlDocument xmlDoc = ReadXml(file);
            string[] arr = GetFields();
            Dictionary<string, string> ArrDataXml = new Dictionary<string, string>(arr.Count());

            foreach (string item in arr)
            {
                ArrDataXml.Add(item, GetFieldTextXml(item, xmlDoc));
            }

            return ArrDataXml;
        }
        protected string GetFieldTextXml(string Field, XmlDocument xmlDoc)
        {
            string text = "";
            if (xmlDoc.GetElementsByTagName(Field, "*")[0] != null)
            {
                text = xmlDoc.GetElementsByTagName(Field, "*")[0].InnerText;
            }
            return text;
        }

        protected string[] GetFields()
        {
            string[] Fields = {
                "Comment",
                "eventComment",
                "statusCode",
                "MessageType",
                "ReferenceMessageID",
                "orderId",
                "Code_smop",
                "IssuingPointAddress",
                "AttachmentFSLink",
                "AttachmentSignatureFSLink",
                "uuid",
                "Hash",
                "MimeType",
                "UserName",
                "Password",
                "FileName",
                "IsUnstructuredFormat",
                "IsZippedPacket",
                "InsuranceRegionCode",
                "PolicyCarrierTypeCode",
                "ReplyTo",
                "MessageID",
                "TransactionCode",
                "RegionCode",
                "SNILS",
                "FamilyName",
                "FirstName",
                "Patronymic",
                "BirthDate",
                "Gender",
                "IdentityDocument",
                "Series",
                "Number",
                "IssueDate",
                "IssuerCode",
                "Nam_smok",
                "FullAddressText",
                "RegionName",
                "City",
                "Street",
                "House",
                "Building",
                "HouseStructure",
                "Flat",
                "PostalIndex",
                "RegDate",
                "IsStayAddressTheSame",
                "ApplicationDate",
                "Email",
                "Citizenship",
                "PhoneNumberRFType",
                "BirthPlace",
                "UnitedPolicyNumber",
                "DateFrom",
                "DateTo",
                "To",
                "OriginalMessageId",
                "Snils"
            };

            return Fields;
        }
    }
}
