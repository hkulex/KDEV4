using System.Linq;

namespace alternatereality
{
	public class Encryption
	{
        public static string Md5Sum(string strToEncrypt)
        {
            System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();

            byte[] bytes = ue.GetBytes(strToEncrypt);
            
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashBytes = md5.ComputeHash(bytes);
            string hashString = hashBytes.Aggregate("", (current, t) => current + System.Convert.ToString(t, 16).PadLeft(2, '0'));

            return hashString.PadLeft(32, '0');
        }
    }
}