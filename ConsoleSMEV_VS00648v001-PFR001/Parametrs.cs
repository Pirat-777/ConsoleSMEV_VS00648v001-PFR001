using System.IO;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

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
        public static IEnumerable<KeyValuePair<string,string>> Get(string section)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("settings.json");

            AppConfiguration = builder.Build();
            var result = AppConfiguration.GetSection(section).AsEnumerable();

            return result;
        }

        private static IConfiguration AppConfiguration { get; set; }
    }
}
