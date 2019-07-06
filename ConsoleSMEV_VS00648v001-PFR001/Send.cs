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
        public static bool Go(string file = null, string InFileFromSmev = null)
        {
            bool res = false;
            if (file != null)
            {   
                try
                {
                    file = file.Replace("WebResponse_", "");

                    // Создаем новый документ XML.
                    XmlDocument doc = new XmlDocument
                    {
                        PreserveWhitespace = true
                    };
                
                    // Читаем документ из файла.
                    using (XmlTextReader XmlTextR = new XmlTextReader(file))
                    {
                        doc.Load(XmlTextR);
                    }

                    //Console.WriteLine("Вебзапрос");
                    ServicePointManager.MaxServicePointIdleTime = 6000;
                    ServicePointManager.DefaultConnectionLimit = 100;

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://smev3-n0.test.gosuslugi.ru:7500/smev/v1.2/ws?wsdl");
                    request.Method = "POST";
                    //request.KeepAlive = true;                                     
                    request.ContentType = "text/xml;charset=\"utf-8\"";
                    //request.ProtocolVersion = HttpVersion.Version11;

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


                    using (StreamWriter sw = new StreamWriter(request.GetRequestStream()))
                    {
                        sw.WriteLine(postData);
                    }

                    HttpWebResponse httpResponse = (HttpWebResponse)request.GetResponse();

                    using (StreamReader swresp = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                    {
                        string text = swresp.ReadToEnd();
                       
                        File.AppendAllText(
                            Paths.In()
                            + "Response_"
                            + Path.GetFileName(file),
                            text
                            );
                       
                        httpResponse.Close();                        
                    }
                    res = true;
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
                                //File.AppendAllText(
                                //        inPath + "Err\\"
                                //        + "Err_WebResponse_"
                                //        + Path.GetFileName(file),
                                //        text
                                //        );
                                Console.WriteLine(text);
                            }
                        }
                        errResp.Close();
                    }
                }
                catch (Exception ex)
                {
                    //File.AppendAllText(
                    //            inPath + "Err\\"
                    //            + "Err_WebResponse_"
                    //            + Path.GetFileNameWithoutExtension(file) + ".txt",
                    //            "\n" + ex.Message
                    //            );
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine("SendMsg: Нет файла для отправки в СМЭВ");
            }
            return res;
        }
    }
}
