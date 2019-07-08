using System;
using System.IO;
using System.Linq;

namespace ConsoleSMEV_VS00648v001_PFR001
{
    static class Paths
    {
        private static readonly string pathApp = Path.GetFullPath(Environment.CurrentDirectory);

        public static void CreateFolder()
        {
            Directory.CreateDirectory(pathApp + "\\" + Parametrs.Get("path:out").First().Value);
            Directory.CreateDirectory(pathApp + "\\" + Parametrs.Get("path:in").First().Value);
            Directory.CreateDirectory(pathApp + "\\" + Parametrs.Get("path:request").First().Value);
            Directory.CreateDirectory(pathApp + "\\" + Parametrs.Get("path:response").First().Value);
            Directory.CreateDirectory(pathApp + "\\" + Parametrs.Get("path:err").First().Value);
            Directory.CreateDirectory(pathApp + "\\" + Parametrs.Get("path:ack").First().Value);
        }

        public static string Ack(string file = "")
        {
            return Path.GetFullPath(pathApp
                + "\\"
                + Parametrs.Get("path:ack").First().Value
                + "\\"                
                + Path.GetFileName(file)
                );
        }

        public static string Request(string file="")
        {
            return Path.GetFullPath(pathApp 
                + "\\" 
                + Parametrs.Get("path:request").First().Value 
                + "\\"                
                + Path.GetFileName(file)
                );
        }

        public static string Response(string file = "")
        {
            return Path.GetFullPath(pathApp 
                + "\\" 
                + Parametrs.Get("path:response").First().Value 
                + "\\" 
                + Path.GetFileName(file)
                );
        }

        public static string Out(string file = "")
        {
            return Path.GetFullPath(pathApp 
                + "\\" 
                + Parametrs.Get("path:out").First().Value 
                + "\\"
                + new CurrentTime().Get() + "_"
                + Path.GetFileName(file)
                );
        }

        public static string In(string file = "")
        {
            return Path.GetFullPath(pathApp 
                + "\\" 
                + Parametrs.Get("path:in").First().Value 
                + "\\"
                + new CurrentTime().Get() + "_"
                + Path.GetFileName(file)
                );
        }
        public static string Err(string file = "")
        {
            return Path.GetFullPath(pathApp 
                + "\\" 
                + Parametrs.Get("path:err").First().Value 
                + "\\"
                + new CurrentTime().Get() + "_"
                + Path.GetFileName(file)
                );
        }
        public static string App()
        {
            return Path.GetFullPath(pathApp + "\\");
        }
        public static string SigFileName(string file)
        {
            return Path.GetFullPath( Out( "\\sig_" + Path.GetFileName(file)) );
        }
    }
}
