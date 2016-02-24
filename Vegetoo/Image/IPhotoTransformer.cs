using System;
using System.IO;
using System.Threading.Tasks;

namespace Wiggin.Drawing
{
	public interface IPhotoTransformer
	{
		Task<Stream> TransformPhotoAsync(Func<byte, byte, byte, double> pixelOperation, string imagePath);

		Task<Stream> ResizePhotoAsync(float maxWidth, float maxHeight, string imagePath);
	}
}

