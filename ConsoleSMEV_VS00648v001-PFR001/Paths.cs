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
            Directory.CreateDirectory(pathApp +"\\"+ "out");
            Directory.CreateDirectory(pathApp + "\\" + "in");
            Directory.CreateDirectory(pathApp + "\\" + "requestShablon");
            Directory.CreateDirectory(pathApp + "\\" + "response");
        }

        public static string Request()
        {
            return (pathApp + "\\" + Parametrs.Get("path:request").First().Value + "\\");
        }

        public static string Response()
        {
            return (pathApp + "\\" + Parametrs.Get("path:response").First().Value + "\\");
        }

        public static string Out()
        {
            return (pathApp + "\\"+Parametrs.Get("path:out").First().Value + "\\");
        }

        public static string In()
        {
            return (pathApp + "\\"+Parametrs.Get("path:in").First().Value + "\\");
        }
        public static string App()
        {
            return (pathApp);
        }
    }
}
