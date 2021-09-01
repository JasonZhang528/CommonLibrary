using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Encryption
{
    /// <summary>
    /// 加密器（单例模式）
    /// </summary>
    /// <remarks>
    /// 包含MD5
    /// </remarks>
    public class Encryptor
    {
        private static Encryptor _encryptor;

        /// <summary>
        /// Constructor
        /// </summary>
        private Encryptor()
        {

        }

        /// <summary>
        /// 获取加密器实例
        /// </summary>
        /// <returns></returns>
        public static Encryptor GetInstance() => _encryptor ?? new();

        #region MD5

        /// <summary>
        /// md5加密
        /// </summary>
        /// <param name="source">要加密的明文</param>
        /// <remarks>
        /// <para>md5加密特点：</para>
        /// <para>1.相同原文加密后的结果是一样的</para>
        /// <para>2.不同长度的内容加密后长度是一样的</para>
        /// <para>3.加密不可逆，不能通过密文解密出原文</para>
        /// <para>4.原文差别很小，但加密后的结果差别很大</para>
        /// <para>5.文件也可以经过加密产生摘要</para>
        /// </remarks>
        /// <returns>密文</returns>
        public string MD5Encrypt(string source)
        {
            try
            {
                MD5 md5 = MD5.Create();
                byte[] data = md5.ComputeHash(Encoding.Default.GetBytes(source));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取文件的MD5摘要
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <returns>文件摘要</returns>
        public string AbstractFile(string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Open))
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(stream);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }

                return sb.ToString();
            }
        }

        #endregion

        #region DES

        /// <summary>
        /// DES加密算法密钥
        /// </summary>
        /// <remarks>如使用DES加密算法，DesKey不可为空</remarks>
        public string DesKey { get; set; }

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="text">需要加密的明文</param>
        /// <param name="keyLen">密钥长度</param>
        /// <remarks>
        /// <para>DES加密特点：</para>
        /// <para>1.加密后能解密回原文，加密key和解密key是同一个</para>
        /// <para>2.加密解密的速度快，问题是密钥的安全性</para>
        /// </remarks>
        /// <returns>密文</returns>
        public string DesEncrypt(string text, int keyLen = 8)
        {
            if (string.IsNullOrWhiteSpace(DesKey))
            {
                throw new ArgumentNullException($"DES加密算法：密钥{nameof(DesKey)} 不能为空！");
            }
            DESCryptoServiceProvider dsp = new DESCryptoServiceProvider();
            using (MemoryStream memStream = new MemoryStream())
            {
                byte[] _rgbKey = ASCIIEncoding.ASCII.GetBytes(DesKey.Substring(0, keyLen));
                byte[] _rgbIV = ASCIIEncoding.ASCII.GetBytes(DesKey.Substring(0, keyLen));
                CryptoStream crypStream = new CryptoStream(memStream, dsp.CreateEncryptor(_rgbKey, _rgbIV), CryptoStreamMode.Write);
                StreamWriter sWriter = new StreamWriter(crypStream);
                sWriter.Write(text);
                sWriter.Flush();
                crypStream.FlushFinalBlock();
                memStream.Flush();
                return Convert.ToBase64String(memStream.GetBuffer(), 0, (int)memStream.Length);
            }
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="encryptText">需要解密的密文</param>
        /// <param name="keyLen">密钥长度</param>
        /// <remarks>
        /// <para>DES加密特点：</para>
        /// <para>1.加密后能解密回原文，加密key和解密key是同一个</para>
        /// <para>2.加密解密的速度快，问题是密钥的安全性</para>
        /// </remarks>
        /// <returns>明文</returns>
        public string DesDecrypt(string encryptText, int keyLen = 8)
        {
            if (string.IsNullOrWhiteSpace(DesKey))
            {
                throw new ArgumentNullException($"DES加密算法：密钥{nameof(DesKey)} 不能为空！");
            }
            DESCryptoServiceProvider dsp = new DESCryptoServiceProvider();
            byte[] buffer = Convert.FromBase64String(encryptText);
            using (MemoryStream memStream = new MemoryStream())
            {
                byte[] _rgbKey = ASCIIEncoding.ASCII.GetBytes(DesKey.Substring(0, keyLen));
                byte[] _rgbIV = ASCIIEncoding.ASCII.GetBytes(DesKey.Substring(0, keyLen));
                CryptoStream crypStream = new CryptoStream(memStream, dsp.CreateDecryptor(_rgbKey, _rgbIV), CryptoStreamMode.Write);
                crypStream.Write(buffer, 0, buffer.Length);
                crypStream.FlushFinalBlock();
                memStream.Flush();
                return ASCIIEncoding.UTF8.GetString(memStream.ToArray());
            }
        }

        #endregion

        #region RSA

        /// <summary>
        /// RSA加密算法密钥
        /// </summary>
        public static KeyValuePair<string, string> RSAKey;

        /// <summary>
        /// RSA加密算法:随机生成一对密钥
        /// </summary>
        /// <returns>true=>生成成功;false=>生成失败</returns>
        public bool GenerateRSAKey()
        {
            try
            {
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                string publicKey = RSA.ToXmlString(false);
                string privateKey = RSA.ToXmlString(true);
                RSAKey = new KeyValuePair<string, string>(publicKey, privateKey);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="plaintext">明文</param>
        /// <remarks>
        /// <para>明文内容+加密密钥(RSAKey.Key)</para>
        /// <para>RSA加密特点：</para>
        /// <para>1.加密后能解密回原文，加密key和解密key不是同一个</para>
        /// <para>2.加密解密速度不快，但安全性好</para>
        /// <para>3.公开加密key，保证数据的安全传递</para>
        /// <para>4.公开解密key，保证数据的不可抵赖</para>
        /// </remarks>
        /// <returns>密文</returns>
        public string RSAEncrypt(string plaintext)
        {
            if (string.IsNullOrWhiteSpace(RSAKey.Key))
            {
                throw new ArgumentNullException($"RSA加密算法：必须先生成密钥{nameof(RSAKey)}！");
            }
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(RSAKey.Key);
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            byte[] DataToEncrypt = ByteConverter.GetBytes(plaintext);
            byte[] resultBytes = rsa.Encrypt(DataToEncrypt, false);
            return Convert.ToBase64String(resultBytes);
        }

        /// <summary>
        /// RSA解密(内容+解密key)
        /// </summary>
        /// <param name="ciphertext">密文</param>
        /// <remarks>
        /// <para>密文内容+加密密钥(RSAKey.Value)</para>
        /// <para>RSA加密特点：</para>
        /// <para>1.加密后能解密回原文，加密key和解密key不是同一个</para>
        /// <para>2.加密解密速度不快，但安全性好</para>
        /// <para>3.公开加密key，保证数据的安全传递</para>
        /// <para>4.公开解密key，保证数据的不可抵赖</para>
        /// </remarks>
        /// <returns>明文</returns>
        public string RSADecrypt(string ciphertext)
        {
            if (string.IsNullOrWhiteSpace(RSAKey.Value))
            {
                throw new ArgumentNullException($"RSA加密算法：必须先生成密钥{nameof(RSAKey)}！");
            }
            byte[] dataToDecrypt = Convert.FromBase64String(ciphertext);
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            RSA.FromXmlString(RSAKey.Value);
            byte[] resultBytes = RSA.Decrypt(dataToDecrypt, false);
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            return ByteConverter.GetString(resultBytes);
        }

        #endregion

    }
}
