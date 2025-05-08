using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Tesseract;
namespace Dir
{

	public class LockBitmap : IDisposable
	{

		public void Dispose()
		{
			UnlockBits();
		}

		public Bitmap source = null;
		IntPtr Iptr = IntPtr.Zero;
		BitmapData bitmapData = null;

		public byte[] Pixels { get; set; }
		public int Depth { get; private set; }
		public int Width { get; private set; }
		public int Height { get; private set; }

		public LockBitmap(Bitmap source)
		{
			this.source = source;
			LockBits();
		}

		/// <summary>
		/// Lock bitmap data
		/// </summary>
		private void LockBits()
		{
			try {
				// Get width and height of bitmap
				Width = source.Width;
				Height = source.Height;

				// get total locked pixels count
				int PixelCount = Width * Height;

				// Create rectangle to lock
				Rectangle rect = new Rectangle(0, 0, Width, Height);

				// get source bitmap pixel format size
				Depth = System.Drawing.Bitmap.GetPixelFormatSize(source.PixelFormat);

				// Check if bpp (Bits Per Pixel) is 8, 24, or 32
				if (Depth != 8 && Depth != 24 && Depth != 32) {
					throw new ArgumentException("Only 8, 24 and 32 bpp images are supported.");
				}

				// Lock bitmap and return bitmap data
				bitmapData = source.LockBits(rect, ImageLockMode.ReadWrite,
					source.PixelFormat);

				// create byte array to copy pixel values
				int step = Depth / 8;
				Pixels = new byte[PixelCount * step];
				Iptr = bitmapData.Scan0;

				// Copy data from pointer to array
				Marshal.Copy(Iptr, Pixels, 0, Pixels.Length);
			} catch (Exception ex) {
				throw ex;
			}
		}

		/// <summary>
		/// Unlock bitmap data
		/// </summary>
		private void UnlockBits()
		{
			try {
				// Copy data from byte array to pointer
				Marshal.Copy(Pixels, 0, Iptr, Pixels.Length);

				// Unlock bitmap data
				source.UnlockBits(bitmapData);
			} catch (Exception ex) {
				throw ex;
			}
		}

		/// <summary>
		/// Get the color of the specified pixel
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public Color GetPixel(int x, int y)
		{

			// Get color components count
			int cCount = Depth / 8;

			// Get start index of the specified pixel
			int i = ((y * Width) + x) * cCount;

			if (i > Pixels.Length - cCount)
				throw new IndexOutOfRangeException();

			if (Depth == 32) { // For 32 bpp get Red, Green, Blue and Alpha
				byte b = Pixels[i];
				byte g = Pixels[i + 1];
				byte r = Pixels[i + 2];
				byte a = Pixels[i + 3]; // a
				return Color.FromArgb(a, r, g, b);
			}
			if (Depth == 24) { // For 24 bpp get Red, Green and Blue
				byte b = Pixels[i];
				byte g = Pixels[i + 1];
				byte r = Pixels[i + 2];
				return Color.FromArgb(r, g, b);
			}
			if (Depth == 8) {            // For 8 bpp get color value (Red, Green and Blue values are the same)
				byte c = Pixels[i];
				return Color.FromArgb(c, c, c);
			}

			return Color.Empty;
		}

		/// <summary>
		/// Get the red color of the specified pixel
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public int GetRedColor(int x, int y)
		{

			// Get color components count
			int cCount = Depth / 8;

			// Get start index of the specified pixel
			int i = ((y * Width) + x) * cCount;

			if (i > Pixels.Length - cCount)
				throw new IndexOutOfRangeException();

			if (Depth == 32 || Depth == 24) { // For 32 bpp get Red, Green, Blue and Alpha
				byte r = Pixels[i + 2];
				return r;
			}
			if (Depth == 8) {            // For 8 bpp get color value (Red, Green and Blue values are the same)
				byte c = Pixels[i];
				return c;
			}

			return 0;
		}

		/// <summary>
		/// Set the color of the specified pixel
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="color"></param>
		public void SetPixel(int x, int y, Color color)
		{
			// Get color components count
			int cCount = Depth / 8;

			// Get start index of the specified pixel
			int i = ((y * Width) + x) * cCount;

			if (Depth == 32) { // For 32 bpp set Red, Green, Blue and Alpha
				Pixels[i] = color.B;
				Pixels[i + 1] = color.G;
				Pixels[i + 2] = color.R;
				Pixels[i + 3] = color.A;
			}
			if (Depth == 24) { // For 24 bpp set Red, Green and Blue
				Pixels[i] = color.B;
				Pixels[i + 1] = color.G;
				Pixels[i + 2] = color.R;
			}
			if (Depth == 8) {            // For 8 bpp set color value (Red, Green and Blue values are the same)
				Pixels[i] = color.B;
			}
		}

	}
	public class PixelUtil
	{
		Bitmap source = null;
		IntPtr Iptr = IntPtr.Zero;
		BitmapData bitmapData = null;

		public byte[] Pixels { get; set; }
		public int Depth { get; private set; }
		public int Width { get; private set; }
		public int Height { get; private set; }

		/// <summary>
		/// Pixel marshaling class, use this to get and set pixels rapidly.
		/// </summary>
		/// <param name="source">The Bitmap to work with</param>
		public PixelUtil(Bitmap source)
		{
			this.source = source;
		}

		/// <summary>
		/// Lock bitmap data
		/// </summary>
		public void LockBits()
		{
			try {
				// Get width and height of bitmap
				Width = source.Width;
				Height = source.Height;

				// get total locked pixels count
				int PixelCount = Width * Height;

				// Create rectangle to lock
				var rect = new Rectangle(0, 0, Width, Height);

				// get source bitmap pixel format size
				Depth = System.Drawing.Image.GetPixelFormatSize(source.PixelFormat);

				// Check if bpp (Bits Per Pixel) is 8, 24, or 32
				if (Depth != 8 && Depth != 24 && Depth != 32) {
					throw new ArgumentException("Only 8, 24 and 32 bpp images are supported.");
				}

				// Lock bitmap and return bitmap data
				bitmapData = source.LockBits(rect, ImageLockMode.ReadWrite,
					source.PixelFormat);

				// create byte array to copy pixel values
				int step = Depth / 8;
				Pixels = new byte[PixelCount * step];
				Iptr = bitmapData.Scan0;

				// Copy data from pointer to array
				Marshal.Copy(Iptr, Pixels, 0, Pixels.Length);
			} catch (Exception) {
				throw;
			}
		}

		/// <summary>
		/// Unlock bitmap data
		/// </summary>
		public void UnlockBits()
		{
			try {
				// Copy data from byte array to pointer
				Marshal.Copy(Pixels, 0, Iptr, Pixels.Length);

				// Unlock bitmap data
				source.UnlockBits(bitmapData);
			} catch (Exception) {
				throw;
			}
		}

		/// <summary>
		/// Get the color of the specified pixel
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public Color GetPixel(int x, int y)
		{
			Color clr = Color.Empty;

			// Get color components count
			int cCount = Depth / 8;

			// Get start index of the specified pixel
			int i = ((y * Width) + x) * cCount;

			if (i > Pixels.Length - cCount)
				throw new IndexOutOfRangeException();

			if (Depth == 32) { //For 32 bpp get Red, Green, Blue and Alpha
				byte b = Pixels[i];
				byte g = Pixels[i + 1];
				byte r = Pixels[i + 2];
				byte a = Pixels[i + 3]; // a
				clr = Color.FromArgb(a, r, g, b);
			}
			if (Depth == 24) { //For 24 bpp get Red, Green and Blue
				byte b = Pixels[i];
				byte g = Pixels[i + 1];
				byte r = Pixels[i + 2];
				clr = Color.FromArgb(r, g, b);
			}
			if (Depth == 8) { //For 8 bpp get color value (Red, Green and Blue values are the same)
				byte c = Pixels[i];
				clr = Color.FromArgb(c, c, c);
			}
			return clr;
		}

		/// <summary>
		/// Set the color of the specified pixel
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="color"></param>
		public void SetPixel(int x, int y, Color color)
		{
			//Get color components count
			int cCount = Depth / 8;

			//Get start index of the specified pixel
			int i = ((y * Width) + x) * cCount;

			if (Depth == 32) { //For 32 bpp set Red, Green, Blue and Alpha
				Pixels[i] = color.B;
				Pixels[i + 1] = color.G;
				Pixels[i + 2] = color.R;
				Pixels[i + 3] = color.A;
			}
			if (Depth == 24) { //For 24 bpp set Red, Green and Blue
				Pixels[i] = color.B;
				Pixels[i + 1] = color.G;
				Pixels[i + 2] = color.R;
			}
			if (Depth == 8) { //For 8 bpp set color value (Red, Green and Blue values are the same)
				Pixels[i] = color.B;
			}
		}
	}
	public static class Images
	{
	
		public static Image Despeckle(this Image image, int maxSpotSize)
		{
			return ((Bitmap)image).Despeckle(maxSpotSize);
		}
		public static Bitmap Despeckle(this Bitmap bitmap, int maxSpotSize)
		{
			if (maxSpotSize == 0) {
				return bitmap;
			}

			Bitmap outputBitmap = new Bitmap(bitmap);

			using (LockBitmap outputLockBitmap = new LockBitmap(outputBitmap)) {
				int width = outputBitmap.Width;
				int height = outputBitmap.Height;

				for (int x = 0; x < width; x++) {
					for (int y = 0; y < height; y++) {
						// Check if the pixel is white
						int pixelRed = outputLockBitmap.GetRedColor(x, y);
						if (pixelRed == 255) {
							//You can turn this on if you also like to fill white spots in black areas (this will take more cpu time)
							//outputLockBitmap.SpotFill(x, y, Color.Black, maxSpotSize);
						} else if (pixelRed == 0) {
							outputLockBitmap.SpotFill(x, y, Color.White, maxSpotSize);
						}
					}
				}

			}

			return outputBitmap;
		}
		public static int SpotFill(this LockBitmap bitmap, int startX, int startY, Color fillColor, int maxSpotSize)
		{
			int targetColor = bitmap.GetRedColor(startX, startY);

			int pixelCount = 0;

			int width = bitmap.Width;
			int height = bitmap.Height;

			// Search for connected pixels, and save them
			int startPixel = startX * width + startY;

			Queue<int> queue = new Queue<int>(maxSpotSize);
			queue.Enqueue(startPixel);

			HashSet<int> changedPixels = new HashSet<int>();

			while (queue.Count > 0) {
				int pixel = queue.Dequeue();
				int x = pixel / width;
				int y = pixel % width;

				if (x < 0 || x >= width || y < 0 || y >= height)
					continue;

				if (bitmap.GetRedColor(x, y) != targetColor)
					continue;

				if (changedPixels.Contains(pixel))
					continue;

				changedPixels.Add(pixel);

				pixelCount++;

				if (pixelCount > maxSpotSize)
					return 0;

				queue.Enqueue((x + 1) * width + y);
				queue.Enqueue((x - 1) * width + y);
				queue.Enqueue(x * width + (y + 1));
				queue.Enqueue(x * width + (y - 1));
			}

			// Apply the fill
			if (pixelCount < maxSpotSize) {
				foreach (int pixel in changedPixels) {
					int x = pixel / width;
					int y = pixel % width;
					bitmap.SetPixel(x, y, fillColor);
				}
				return pixelCount;
			}

			return 0;
		}
		public static byte[] ScreenShoot(Point p1, Point p2, int threshold = 128)
		{
//			int screenLeft = SystemInformation.VirtualScreen.Left;
//			int screenTop = SystemInformation.VirtualScreen.Top;
//			int screenWidth = SystemInformation.VirtualScreen.Width;
//			int screenHeight = SystemInformation.VirtualScreen.Height;
		
			
// Create a bitmap of the appropriate size to receive the full-screen screenshot.
			using (Bitmap bitmap = new Bitmap(p2.X - p1.X, p2.Y - p1.Y)) {
				// Draw the screenshot into our bitmap.
				using (Graphics g = Graphics.FromImage(bitmap)) {
					g.CopyFromScreen(p1.X, p1.Y, 0, 0, bitmap.Size);
				}

				var ms = new MemoryStream();
				var b = bitmap;
				//var b = bitmap.Despeckle(maxSpotSize: 1);
				if (threshold == 0) {
					b.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
					b.Dispose();
					return ms.ToArray();
				}
				PixelUtil pixelUtil = new PixelUtil(b);
				pixelUtil.LockBits();
			
				for (int i = 0; i < b.Width; i++) {
					for (int j = 0; j < b.Height; j++) {
						Color firstPixel = pixelUtil.GetPixel(i, j);

						if (firstPixel.R > threshold)
							pixelUtil.SetPixel(i, j, Color.Black);
						else
							pixelUtil.SetPixel(i, j, Color.White);
					}
				}

			

				//Don't forget to unlock!
				pixelUtil.UnlockBits();
				b.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
				b.Dispose();
				return ms.ToArray();
			}
		}
		static string ProcessValue(string s)
		{
			var number = Regex.Match(s, "[.\\d-]+");
			if (number.Success) {
//			try {
//				Clipboard.SetText(float.Parse(number.Value).ToString());
//			} catch {
				var str = number.Value;
				if (str.StartsWith("0") && !str.StartsWith("0.")) {
					Clipboard.SetText("0." + str.Substring(1));
				} else
					Clipboard.SetText(number.Value);
			 
				//}
			}
			return s;
		}
	
		static TesseractEngine _engine;
		public static void Ocr(MainForm fv, TextBox textBox1, int threshold = 128, int xOffset = 90, int yOffset = 20, bool isNumber = true)
		{
			try {
				Screenshot.POINT p = new Screenshot.POINT();
				Screenshot.GetCursorPos(out p);
				//Ocr(new Point(p.X, p.Y), new Point(p.X + 260, p.Y + 30));
				if (_engine == null)
					_engine = new TesseractEngine("./traineddata".GetEntryPath(), "eng", EngineMode.Default);
				var buf = Images.ScreenShoot(new Point(p.X, p.Y), new Point(p.X + xOffset, p.Y + yOffset), threshold);
				//var f = new FileStream("3.png".GetDesktopPath(), FileMode.OpenOrCreate);
				//f.Write(buf, 0, buf.Length);
				//f.Dispose();
				using (var img = Pix.LoadFromMemory(buf)) {
					using (var page = _engine.Process(img)) {
						var text = page.GetText();
//						Console.WriteLine("Mean confidence: {0}", page.GetMeanConfidence());
//
//						Console.WriteLine("Text (GetText): \r\n{0}", text);
//						Console.WriteLine("Text (iterator):");
//						
								
					
						fv.Invoke(new Action(() => {
							if (string.IsNullOrWhiteSpace(text)) {
								_engine = new TesseractEngine("./traineddata".GetEntryPath(), "eng", EngineMode.Default);
								return;
							}
							textBox1.SelectedText = Environment.NewLine + Environment.NewLine + text + Environment.NewLine;
							
						
							if (isNumber) {
								fv.Text = ProcessValue(text);
							} else {
								Clipboard.SetText(text);
							}
					
						}));
					}
				}
			} catch(Exception e) {
			
				_engine = new TesseractEngine("./traineddata".GetEntryPath(), "eng", EngineMode.Default);
			}
		}

	}
}