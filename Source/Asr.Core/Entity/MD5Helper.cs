using System;
using System.Security.Cryptography;
using System.Text;

namespace AsrLibrary.Entity
{
    internal class MD5Helper
    {
        /// <summary>
        /// 16位MD5加密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string MD5Encrypt16(string text)
        {
            var md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(text)), 4, 8);
            t2 = t2.Replace("-", "");

            return t2;
        }

        /// <summary>
        /// 32为MD5加密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string MD5Encrypt32(string text)
        {
            string t2 = "";
            MD5 md5 = MD5.Create();  // 实例化一个md5对象
            // 加密后是一个字节型的数组，这里要注意编码的选择
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
            // 通过使用循环，将字节类型的数据转换为字符串，此字符串是常规格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符串是小写的字母，如果使用大写（X）则格式化后的字符是大写字符
                t2 += s[i].ToString("X");
            }

            return t2;
        }

        public static string MD5Encrypt64(string text)
        {
            MD5 md5 = MD5.Create();
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(text));

            return Convert.ToBase64String(s);
        }

        public static string getXSessionKey(string currTime, string developerKey)
        {
            MD5 md5 = MD5.Create();
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(currTime + developerKey));
            return byteToHex(s);
        }

        public static string byteToHex(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            int num;

            for (int count = 0; count < bytes.Length; count++)
            {
                num = bytes[count];
                if (num < 0)
                {
                    num += 256;
                }
                if (num < 16)
                {
                    sb.Append("0");
                }
                sb.Append(num.ToString("X"));
            }

            return sb.ToString().ToUpper();
        }
    }
}
