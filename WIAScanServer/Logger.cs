using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace WIAScanServer
{
    public class Logger
    {
        private const String fileName = "D:\\scan\\log.txt";

        private static bool firstLine = true;
        private static StreamWriter log;

        public static void Log(String str)
        {
            if (!File.Exists(fileName))
            {
                log = new StreamWriter(fileName);
            }
            else
            {
                log = File.AppendText(fileName);
            }

            if (firstLine)
            {
                log.WriteLine();
                firstLine = false;
            }

            log.WriteLine(DateTime.Now + ": " + str);
            log.Close();
        }

    }
}