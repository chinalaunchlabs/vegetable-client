using System;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using Wiggin.Drawing.iOS;
using Wiggin.Drawing;

[assembly:Xamarin.Forms.Dependency(typeof(PhotoTransformer))]
namespace Wiggin.Drawing.iOS
{
	public class PhotoTransformer: IPhotoTransformer
	{
		public PhotoTransformer ()
		{
		}

		public Task<Stream> TransformPhotoAsync(Func<byte, byte, byte, double> pixelOperation, string imagePath) {
			return Task.Run (() => {
				var bitmap = new Bitmap();
				bitmap.LoadImage(new UIKit.UIImage(imagePath));
				bitmap.ToPixelArray();
				bitmap.TransformImage(pixelOperation);
				return bitmap.ToImage().AsJPEG().AsStream();
			});
		}

		public Task<Stream> ResizePhotoAsync(float maxWidth, float maxHeight, string imagePath) {
			return Task.Run (() => {
				var bitmap = new Bitmap();
				bitmap.LoadImage(new UIKit.UIImage(imagePath));
				return bitmap.ResizeImage(maxWidth, maxHeight).AsJPEG().AsStream();
			});
		}
	}
}

