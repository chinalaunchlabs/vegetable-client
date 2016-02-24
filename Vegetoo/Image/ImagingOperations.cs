using System;

namespace Wiggin.Drawing
{
	public static class ImagingOperations
	{
		public static Func<byte, byte, byte, double> ConvertToGreyscale = (r, g, b) => (r + g + b)/3;
	}
}
