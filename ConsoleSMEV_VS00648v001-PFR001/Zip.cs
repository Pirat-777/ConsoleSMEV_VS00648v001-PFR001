using System;
using System.IO;
using System.IO.Compression;
using System.Linq;


namespace ConsoleSMEV_VS00648v001_PFR001
{
    class Zip
    {
        //получить файлы и отправить на запаковку
        // inMask - маска файла для поиска
        // inPath - путь к файлам для поиска
        // nameArch - имя создаваемого архива
        public void Compress(string mask, string path, string archName)
        {   
            DirectoryInfo di = new DirectoryInfo(path);
            var mass = di.GetFiles(mask).ToList();
            var massFiles = di.GetFiles(mask).ToList();

            if (mass.Count() == 0)
            {
                Console.WriteLine($"ToZip: Нет файлов в папке {path} для запаковки - маска поиска: {mask}");
            }
            else
            {
                try
                {                    
                    Console.WriteLine($"ToZip: Отправляем файл(ы) из папки {path} {mass.Count()} шт. на запаковку.");
                    
                    foreach (FileInfo filePath in mass)
                    {                    
                        ToCompress((filePath), path + archName + "_" + DateTime.Now.ToString("yyyy-MM-dd") + ".zip");
                    }
                    mass.Clear();

                    //Console.WriteLine($"ToZip: Чистим папку {path}.");
                    //удаляем оригиналы упакованных файлов
                    foreach (FileInfo file in massFiles)
                    {
                        file.Delete();                     
                    }
                    massFiles.Clear();

                    Console.WriteLine($"ToZip: выполнено.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadKey();
                }
            }
        }

        protected void ToCompress(FileInfo file, string archNAme)
        {
            FileStream zipToOpen = new FileStream(archNAme, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
            {
                archive.CreateEntry(file.FullName);
            }
        }
    }
}
