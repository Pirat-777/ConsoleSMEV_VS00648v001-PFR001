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
            ToZip();

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

        private static void ToZip()
        {
            string[] paths = { Paths.In(), Paths.Out() };
            var masks = Parametrs.Get("maskToZip");

            foreach (var path in paths)
            {
                foreach (var mask in masks)
                {
                    if (mask.Value != null) 
                    {                        
                        if ( (new DirectoryInfo(path).Name == "in" && mask.Value.Contains("GetResponseRequest") == true) != true )
                        {
                            new Zip().Compress(
                                mask.Value,
                                path,
                                new DirectoryInfo(path).Name
                            );
                        }
                        else
                        {
                            foreach (var file in new DirectoryInfo(path).GetFiles(mask.Value).ToList())
                            {
                                File.Move(file.FullName.ToString(), Paths.ForProcessing(Path.GetFileName(file.ToString())));
                            }                             
                        }                        
                    }
                }                
            }
        }
    }
}
