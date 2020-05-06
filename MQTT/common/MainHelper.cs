using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MQTT.common
{
    public class MainHelper
    {
        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataToSign"></param>
        /// <returns></returns>
        public static string HMACSHA1(string key, string dataToSign)
        {
            Byte[] secretBytes = UTF8Encoding.UTF8.GetBytes(key);
            HMACSHA1 hmac = new HMACSHA1(secretBytes);
            Byte[] dataBytes = UTF8Encoding.UTF8.GetBytes(dataToSign);
            Byte[] calcHash = hmac.ComputeHash(dataBytes);
            String calcHashString = Convert.ToBase64String(calcHash);
            return calcHashString;
        }
        /// <summary>
        /// 获取随机字符串
        /// </summary>
        /// <param name="length">长度</param>
        /// <param name="useNum">是否使用数字</param>
        /// <param name="useLow">是否使用小写字母</param>
        /// <param name="useUpp">是否使用大写字母</param>
        /// <param name="useSpe">是否使用特殊字符</param>
        /// <param name="custom">固定字符串</param>
        /// <returns></returns>
        public static string GetRandomString(int length, bool useNum, bool useLow, bool useUpp, bool useSpe, string custom)
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = custom;
            if (useNum == true) { str += "0123456789"; }
            if (useLow == true) { str += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUpp == true) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe == true) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }
            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }
            return s;
        }
        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp">Unix时间戳格式</param>
        /// <returns>C#格式时间</returns>
        public static DateTime StampToDateTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            string strZero = "0000000";
            if (timeStamp.Length == 13)
                strZero = "0000";
            long lTime = long.Parse(timeStamp + strZero);
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }
    }
}
