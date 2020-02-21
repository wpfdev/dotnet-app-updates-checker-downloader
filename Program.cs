using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AppUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Count() == 0 || string.IsNullOrEmpty(args[0]))
                System.Diagnostics.Process.GetCurrentProcess().Close();
            else
            {
                string appName = args[0];
                Console.Title = "Обновление приложения " + appName;

                if (UpdatesChecker.CheckUpdates(appName, "1"))
                    UpdatesChecker.DownloadUpdates(appName, "", false);

                Console.WriteLine("Обновление завершено, нажмите любую кнопку...");
                Console.ReadKey();

                //Удаление апдейтера
                string[] bat = { "del " + '"' + System.IO.Path.GetFileName(Assembly.GetExecutingAssembly().Location) + '"', "del %0" };
                System.IO.File.WriteAllLines("delupd.cmd", bat);
                System.Diagnostics.Process.GetCurrentProcess().Close();
                System.Diagnostics.Process.Start("delupd.cmd");
            }
        }
    }
}
