using System.IO;
using Microsoft.Extensions.Configuration;

namespace ConsoleSMEV_VS00648v001_PFR001
{
    static class Parametrs
    {
        /*  пример: в файле json с содержимым
         *  {
         *      "color": "red",
         *      "namespace": { "class": { "method": "AddJson" } }
         *  } 
         *  чтобы обратиться к этой настройке
         *  Parametrs.Get("namespace:class:method");
        */
        public static string Get(string section)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("settings.json");
            AppConfiguration = builder.Build();

            return AppConfiguration[section];
        }

        private static IConfiguration AppConfiguration { get; set; }
    }
}
