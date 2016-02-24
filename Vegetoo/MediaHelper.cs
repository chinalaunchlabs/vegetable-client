using System;
using System.IO;

namespace Vegeta
{
	public static class MediaHelper
	{
		public static byte[] ToByteArray(this Stream stream) {
			MemoryStream ms = new MemoryStream();
			stream.CopyTo(ms);

			return ms.ToArray ();
		}

		public static string Base64Convert(this byte[] bytes) {
			return Convert.ToBase64String (bytes);
		}
	}
}

