using System;
using System.Security.Cryptography;
using System.Text;
namespace GoldTree.Util
{
	public static class EncryptionUtility
	{
		public static string smethod_0(string string_0)
		{
			StringBuilder stringBuilder = new StringBuilder(string_0);
			StringBuilder stringBuilder2 = new StringBuilder(string_0.Length);
			for (int i = 0; i < string_0.Length; i++)
			{
				char c = stringBuilder[i];
				c ^= '\u0081';
				stringBuilder2.Append(c);
			}
			return stringBuilder2.ToString();
		}
		public static string smethod_1(string string_0)
		{
			byte[] bytes = Encoding.ASCII.GetBytes(string_0);
			byte[] array = new MD5CryptoServiceProvider().ComputeHash(bytes);
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("x2"));
			}
			return stringBuilder.ToString();
		}
	}
}
