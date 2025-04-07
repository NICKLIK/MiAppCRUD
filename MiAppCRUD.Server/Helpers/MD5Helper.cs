using System.Security.Cryptography;
using System.Text;



namespace MiAppCRUD.Server.Helpers
{
    public static class MD5Helper
    {
        public static string EncriptarMD5(string input)
        {
            using var md5 = MD5.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // 
            var sb = new StringBuilder();
            foreach (var t in hashBytes)
                sb.Append(t.ToString("X2"));
            return sb.ToString();
        }
    }
}
