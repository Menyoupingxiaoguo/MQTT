using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTT.common
{
    public class LogHelper
    {
        static object locker = new object();
        /// <summary>  
        /// 写入所有日志  
        /// </summary>  
        /// <param name="logs">日志列表，每条日志占一行</param>  
        public static void WriteProgramLog(params string[] logs)
        {
            lock (locker)
            {
                string path = typeof(Program).Assembly.Location; //第一句代码 是获取Program 这个类所在 程序集dll 的物理路径  
                string pro = Path.GetDirectoryName(path); //第二句代码 是获取这个dll程序集所在的目录位置  

                string LogAddress = pro + @"\log";
                if (!Directory.Exists(LogAddress + "\\Log"))
                {
                    Directory.CreateDirectory(LogAddress + "\\Log");
                }
                LogAddress = string.Concat(LogAddress, "\\Log\\",
                 DateTime.Now.Year, '-', DateTime.Now.Month, '-',
                 DateTime.Now.Day, "_Log.log");
                StreamWriter sw = new StreamWriter(LogAddress, true);
                foreach (string log in logs)
                {
                    sw.WriteLine(log + "  at  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                sw.Close();
            }
        }
    }
}
