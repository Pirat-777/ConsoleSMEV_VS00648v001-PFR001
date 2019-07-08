using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleSMEV_VS00648v001_PFR001
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Paths.CreateFolder();

                int period = Convert.ToInt32( Parametrs.Get("periodStart").First().Value );
                var startTimeSpan = TimeSpan.Zero;
                var periodTimeSpan = TimeSpan.FromMinutes(period);

                var timer = new System.Threading.Timer((e) =>
                {
                    Start();
                }, null, startTimeSpan, periodTimeSpan);

            }
            catch (Exception ex)
            {
                Console.WriteLine("err: {0}",ex.Message);
            }            

            Console.WriteLine("end");

            Console.ReadKey();
        }

        private static void Start()
        {
            Send.Go(
                    out _,
                    out _,
                    new PrepareXml().Request(
                            Parametrs.Get("XmlSablon:SendRequestRequest").First().Value
                        )
                    );

            Send.Go(
                out bool GetResult,
                out string GetPathInName,
                new PrepareXml().Request(
                        Parametrs.Get("XmlSablon:GetResponseRequest").First().Value
                    )
                );
            if (GetResult)
            {
                new Ack().Go(GetPathInName);
            }
        }
    }
}
