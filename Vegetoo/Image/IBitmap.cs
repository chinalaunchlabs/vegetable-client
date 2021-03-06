﻿using System;

namespace Wiggin.Drawing
{
	public interface IBitmap<T>
	{
		/// <summary>
		/// Converts the native image type to a byte array of raw pixel data.
		/// </summary>
		void ToPixelArray();

		/// <summary>
		/// Performs an imaging operation on the pixel data 
		/// (eg. convert to greyscale, sepia, color balance, etc).
		/// </summary>
		/// <param name="pixelOperation">The function that will be used to perform per-pixel operations.</param>
		void TransformImage(Func<byte, byte, byte, double> pixelOperation);

		/// <summary>
		/// Resizes the image, keeping the aspect ratio of original.
		/// </summary>
		/// <param name="maxWidth">Max width.</param>
		/// <param name="maxHeight">Max height.</param>
		T ResizeImage(float maxWidth, float maxHeight);

		/// <summary>
		/// Converts the transformed pixel data back to the native image type.
		/// </summary>
		/// <returns>The image.</returns>
		T ToImage();
	}
}

