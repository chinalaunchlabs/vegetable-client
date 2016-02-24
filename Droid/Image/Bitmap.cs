using System;
using Android.Graphics;
using System.Runtime.InteropServices;

[assembly:Xamarin.Forms.Dependency(typeof(Bitmap))]
namespace Wiggin.Drawing.Droid
{
	public class Bitmap: IBitmap<Android.Graphics.Bitmap>
	{
		const byte bytesPerPixel = 4;
		int height; 
		int width;
		byte[] pixelData;
		Android.Graphics.Bitmap bitmap;
		string photoFile;

		public Bitmap (string photo)
		{
			photoFile = photo;
			var options = new BitmapFactory.Options {
				InJustDecodeBounds = true
			};

			// Bitmap will be null because InJustDecodeBounds = true
			bitmap = BitmapFactory.DecodeFile(photoFile, options);
			width = options.OutWidth;
			height = options.OutHeight;
		}

		public void ToPixelArray() {
			bitmap = BitmapFactory.DecodeFile (photoFile);

			int size = width * height * bytesPerPixel;
			pixelData = new byte[size];
			var byteBuffer = Java.Nio.ByteBuffer.AllocateDirect (size);
			bitmap.CopyPixelsToBuffer (byteBuffer);
			Marshal.Copy (byteBuffer.GetDirectBufferAddress (), pixelData, 0, size);
			byteBuffer.Dispose ();
		}

		public void TransformImage(Func<byte, byte, byte, double> pixelOperation) {
			byte r, g, b;

			try {
				// Pixel data order is RGB
				for (int i = 0; i < pixelData.Length; i += bytesPerPixel) {
					r = pixelData[i];
					g = pixelData[i+1];
					b = pixelData[i+2];

					pixelData[i] = pixelData[i+1] = pixelData[i+2] = (byte)pixelOperation(r,g,b);
				}
			} catch (Exception e) {
				Console.WriteLine (e.Message);
			}
		}

		public Android.Graphics.Bitmap ResizeImage(float maxWidth, float maxHeight) {
			bitmap = BitmapFactory.DecodeFile (photoFile);

			float maxResizeFactor = Math.Max (maxWidth / width, maxHeight / height);
			if (maxResizeFactor > 1)
				return bitmap;
			int newWidth = (int)Math.Round(maxResizeFactor * width);
			int newHeight =  (int)Math.Round(maxResizeFactor * height);
			System.Diagnostics.Debug.WriteLine ("new size: {0}x{1}", newWidth, newHeight);

			Android.Graphics.Bitmap newBitmap = Android.Graphics.Bitmap.CreateScaledBitmap (bitmap, newWidth, newHeight, false);
			return newBitmap;
		}

		public Android.Graphics.Bitmap ToImage() {
			var byteBuffer = Java.Nio.ByteBuffer.AllocateDirect (width * height * bytesPerPixel);
			Marshal.Copy (pixelData, 0, byteBuffer.GetDirectBufferAddress (), width * height * bytesPerPixel);
			bitmap.CopyPixelsFromBuffer (byteBuffer);
			byteBuffer.Dispose ();
			return bitmap;
		}

		public void Dispose() {
			if (bitmap != null) {
				bitmap.Recycle ();
				bitmap.Dispose ();
				bitmap = null;
			}
			pixelData = null;
		}
	}
}

