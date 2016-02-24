using System;
using Wiggin.Drawing.Droid;
using System.Threading.Tasks;
using System.IO;

[assembly:Xamarin.Forms.Dependency(typeof(PhotoTransformer))]
namespace Wiggin.Drawing.Droid
{
	public class PhotoTransformer: IPhotoTransformer
	{
		public PhotoTransformer ()
		{
		}

		public Task<Stream> TransformPhotoAsync(Func<byte, byte, byte, double> pixelOperation, string imagePath) {
			return Task.Run (() => {
				var bitmap = new Bitmap(imagePath);
				bitmap.ToPixelArray();
				bitmap.TransformImage(pixelOperation);

				var memoryStream = new MemoryStream();
				var androidBitmap = bitmap.ToImage();
				androidBitmap.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, 80, memoryStream);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				bitmap.Dispose();

				return (Stream)memoryStream;
			});
		}

		public Task<Stream> ResizePhotoAsync(float maxWidth, float maxHeight, string imagePath) {
			return Task.Run (() => {
				var bitmap = new Bitmap(imagePath);
				var memoryStream = new MemoryStream();
				var androidBitmap = bitmap.ResizeImage(maxWidth, maxHeight);
				androidBitmap.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, 80, memoryStream);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				bitmap.Dispose();

				return (Stream)memoryStream;
			});
		}
	}
}

