using System;
using System.Security.Cryptography;
using System.Reflection;
using System.Xml;
using System.Security.Cryptography.Xml;

namespace ConsoleSMEV_VS00648v001_PFR001
{
    class _SignedXml : SignedXml
    {
        public _SignedXml(XmlDocument document)
                : base(document)
        {
        }

        public override XmlElement GetIdElement(XmlDocument document, string idValue)
        {
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(document.NameTable);            
            return document.SelectSingleNode("//*[@Id='" + idValue + "']", nsmgr) as XmlElement;
        }

        public XmlElement GetXml(string prefix)
        {
            XmlElement e = this.GetXml();
            SetPrefix(prefix, e);
            return e;
        }

        private void SetPrefix(string prefix, XmlNode node)
        {
            foreach (XmlNode n in node.ChildNodes)
                SetPrefix(prefix, n);
            node.Prefix = prefix;
        }
        
        public void ComputeSignature(string prefix)
        {
            this.BuildDigestedReferences();

            AsymmetricAlgorithm signingKey = this.SigningKey;
            if (signingKey == null)
            {
                throw new CryptographicException("Cryptography_Xml_LoadKeyFailed");
            }

            if (this.SignedInfo.SignatureMethod == null)
            {
                if (!(signingKey is DSA))
                {
                    if (!(signingKey is RSA))
                    {
                        throw new CryptographicException("Cryptography_Xml_CreatedKeyFailed");
                    }

                    if (this.SignedInfo.SignatureMethod == null)
                    {
                        this.SignedInfo.SignatureMethod = "http://www.w3.org/2000/09/xmldsig#rsa-sha1";
                    }
                }
                else
                {
                    this.SignedInfo.SignatureMethod = "http://www.w3.org/2000/09/xmldsig#dsa-sha1";
                }
            }

            SignatureDescription description = CryptoConfig.CreateFromName(this.SignedInfo.SignatureMethod) as SignatureDescription;
            if (description == null)
            {
                throw new CryptographicException("Cryptography_Xml_SignatureDescriptionNotCreated");
            }

            HashAlgorithm hash = description.CreateDigest();
            if (hash == null)
            {
                throw new CryptographicException("Cryptography_Xml_CreateHashAlgorithmFailed");
            }

            this.GetC14NDigest(hash, prefix);
            this.m_signature.SignatureValue = description.CreateFormatter(signingKey).CreateSignature(hash);
        }

        private byte[] GetC14NDigest(HashAlgorithm hash, string prefix)
        {
            XmlDocument document = new XmlDocument
            {
                PreserveWhitespace = true
            };
            XmlElement e = this.SignedInfo.GetXml();
            document.AppendChild(document.ImportNode(e, true));

            Transform canonicalizationMethodObject = this.SignedInfo.CanonicalizationMethodObject;
            SetPrefix(prefix, document.DocumentElement); //Set the prefix before getting the HASH
            canonicalizationMethodObject.LoadInput(document);
            return canonicalizationMethodObject.GetDigestedOutput(hash);
        }
        
        private void BuildDigestedReferences()
        {
            Type t = typeof(SignedXml);
            MethodInfo m = t.GetMethod("BuildDigestedReferences", BindingFlags.NonPublic | BindingFlags.Instance);
            m.Invoke(this, new object[] { });
        }
    }
}
