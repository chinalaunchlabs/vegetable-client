using System;
using System.Runtime.InteropServices;
using UIKit;
using CoreGraphics;
using Xamarin.Forms;
using Wiggin.Drawing;
using Wiggin.Drawing.iOS;
using System.Drawing;

[assembly:Xamarin.Forms.Dependency(typeof(Bitmap))]
namespace Wiggin.Drawing.iOS
{
	public class Bitmap : IBitmap<UIImage>
	{
		const byte bitsPerComponent = 8;
		const byte bytesPerPixel = 4;
		UIImage image;
		int width;
		int height;
		IntPtr rawData;
		byte[] pixelData;
		UIImageOrientation orientation;

		public Bitmap ()
		{
		}

		public void LoadImage(UIImage uiImage) {
			image = uiImage;
			orientation = image.Orientation;
			width = (int)image.CGImage.Width;
			height = (int)image.CGImage.Height;
		}

		public void ToPixelArray() {
			using (var colorSpace = CGColorSpace.CreateDeviceRGB ()) {
				pixelData = new byte[width * height * bytesPerPixel];
				using (var context = new CGBitmapContext (pixelData, width, height, bitsPerComponent, 
					                     bytesPerPixel * width, colorSpace, CGImageAlphaInfo.PremultipliedLast)) {
					context.DrawImage(new CGRect(0, 0, width, height), image.CGImage);
				}
			}
		}

		public void TransformImage(Func<byte, byte, byte, double> pixelOperation) {
			byte r, g, b;

			// Pixel data order is RGB
			try {
				for (int i = 0; i < pixelData.Length; i += bytesPerPixel) {
					r = pixelData[i];
					g = pixelData[i+1];
					b = pixelData[i+2];

					// Leave alpha value intact
					pixelData[i] = pixelData[i+1] = pixelData[i+2] = (byte)pixelOperation(r, g, b);
				}
			} catch (Exception e) {
				Console.WriteLine (e.Message);
			}
		}

		public UIImage ResizeImage(float maxWidth, float maxHeight) {
			var maxResizeFactor = Math.Max (maxWidth / width, maxHeight / height);
			if (maxResizeFactor > 1)
				return image;
			var newWidth = maxResizeFactor * width;
			var newHeight = maxResizeFactor * height;
			UIGraphics.BeginImageContext (new SizeF (newWidth, newHeight));
			image.Draw (new RectangleF (0, 0, newWidth, newHeight));
			var resultImage = UIGraphics.GetImageFromCurrentImageContext ();
			UIGraphics.EndImageContext ();
			return resultImage;
		}

		public UIImage ToImage() {
			using (var dataProvider = new CGDataProvider (pixelData, 0, pixelData.Length)) {
				using (var colorSpace = CGColorSpace.CreateDeviceRGB ()) {
					using (var cgImage = new CGImage (width, height, bitsPerComponent, 
						                     bitsPerComponent * bytesPerPixel, bytesPerPixel * width, colorSpace,
						                     CGBitmapFlags.ByteOrderDefault, dataProvider, null, false, CGColorRenderingIntent.Default)) {
						image.Dispose ();
						image = null;
						pixelData = null;
						return UIImage.FromImage (cgImage, 0, orientation);
					}
				}
			}
		}
	}
}
