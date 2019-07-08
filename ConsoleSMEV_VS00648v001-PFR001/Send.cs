using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace ConsoleSMEV_VS00648v001_PFR001
{
    static class Send
    {
        public static void Go(out bool result, out string pathInName, string sigFile = null)
        {
            result = false;
            pathInName = "";
            if (sigFile != null)
            {
                string soapTextPattern = "(<soap:envelope.*soap:envelope>)";                
                try
                {
                    // Создаем новый документ XML.
                    XmlDocument doc = new XmlDocument
                    {
                        PreserveWhitespace = true
                    };
                
                    // Читаем документ из файла.
                    using (XmlTextReader XmlTextR = new XmlTextReader(sigFile))
                    {
                        doc.Load(XmlTextR);
                    }
                                        
                    ServicePointManager.MaxServicePointIdleTime = 6000;
                    ServicePointManager.DefaultConnectionLimit = 100;

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Parametrs.Get("adresSmev").First().Value);
                    request.Method = "POST";
                    //request.KeepAlive = true;                                     
                    request.ContentType = "text/xml;charset=\"utf-8\"";
                    //request.ProtocolVersion = HttpVersion.Version11;


                    //для схемы 1.3--------------------
                    string postData = doc.InnerXml;
                    if (Parametrs.Get("soapActionOnOff:enable").First().Value == "1")
                    {
                        string pref = Parametrs.Get("soapSet:prefix").First().Value;
                        string action = "";
                        var iActions = Parametrs.Get("soapAction");                       

                        foreach (var iAction in iActions)
                        {
                            if (Regex.IsMatch(postData, @"(.*" + iAction.Value + ")"))
                            {
                                action = iAction.Value;
                                break;
                            }                            
                        }

                        request.Headers.Add("SOAPAction", pref + action);
                    }
                    //---------------------------------

                    using (StreamWriter sw = new StreamWriter(request.GetRequestStream()))
                    {
                        sw.WriteLine(postData);
                    }

                    HttpWebResponse httpResponse = (HttpWebResponse)request.GetResponse();

                    using (StreamReader swresp = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                    {
                        string text = Regex.Match(
                                        swresp.ReadToEnd()
                                        , soapTextPattern, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace
                                      ).Value;

                        pathInName = Paths.In(
                            "response_"
                            + Path.GetFileName(sigFile)
                        );

                        File.WriteAllText(
                            pathInName,
                            text
                        );
                        httpResponse.Close();                        
                    }
                    result = true;
                }
                catch (WebException ex)
                {
                    HttpWebResponse errResp = (HttpWebResponse)ex.Response;
                    if (errResp != null)
                    {
                        using (Stream respStream = errResp.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(respStream))
                            {
                                string text = reader.ReadToEnd();
                                File.WriteAllText(
                                        Paths.Err(
                                            "err_"
                                            + Path.GetFileName(sigFile)
                                        ),
                                        Regex.Match(text, soapTextPattern, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace).Value
                                        );
                                Console.WriteLine(text);
                            }
                        }
                        errResp.Close();
                    }
                }
                catch (Exception ex)
                {
                    File.WriteAllText(
                                Paths.Err(
                                    "err_"
                                    + Path.GetFileNameWithoutExtension(sigFile) + ".txt"
                                ),
                                "\n" + ex.Message
                                );
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Send: Нет файла для отправки в СМЭВ");
            }
            return result;
        }
    }
}
