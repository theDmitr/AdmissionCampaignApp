using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace AdmissionCampaign.Converters
{
    /// <summary>
    /// Конвертер SecureString в String и наоборот (используя хеширование)
    /// </summary>
    public class SecureStringToHashStringConverter
    {
        public static string ConvertSecureStringToString(SecureString secureString)
        {
            StringBuilder stringBuilder = new();

            unsafe
            {
                nint ptr = IntPtr.Zero;
                try
                {
                    ptr = Marshal.SecureStringToBSTR(secureString);

                    int length = secureString.Length;
                    byte[] bytes = new byte[length * 2];

                    Marshal.Copy(ptr, bytes, 0, bytes.Length);
                    byte[] hashBytes = SHA256.HashData(bytes);

                    foreach (byte b in hashBytes)
                    {
                        _ = stringBuilder.Append(b.ToString("x2"));
                    }

                    return stringBuilder.ToString();
                }
                finally
                {
                    if (ptr != IntPtr.Zero)
                    {
                        Marshal.ZeroFreeBSTR(ptr);
                    }
                }
            }
        }

        public static SecureString ConvertStringToSecureString(string str)
        {
            if (str == null)
            {
                return null;
            }

            SecureString securePassword = new();

            foreach (char c in str)
            {
                securePassword.AppendChar(c);
            }

            securePassword.MakeReadOnly();
            return securePassword;
        }
    }
}
