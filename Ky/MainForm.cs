
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace Ky
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		
		double[] _values = new []{ .1, .2, .25, .3, .4, .5, .8 };
		public MainForm()
		{
			//MergeImages();
			
			InitializeComponent();
			for (int i = 0; i < _values.Length; i++) {
				listBox1.Items.Add((_values[i] * -1).ToString());
			}
			for (int i = 0; i < _values.Length; i++) {
				listBox1.Items.Add(_values[i].ToString());
			}
			for (int i = 0; i < _values.Length; i++) {
				listBox1.Items.Add((_values[i] * 10).ToString());
			}
		}
		void ListBox1DoubleClick(object sender, EventArgs e)
		{
			if (listBox1.SelectedIndex != -1) {
				Clipboard.SetText(listBox1.SelectedItem.ToString());
			}
		}
		private static ImageCodecInfo GetEncoderInfo(string mimeType)
		{
			foreach (ImageCodecInfo codec in ImageCodecInfo.GetImageEncoders())
				if (codec.MimeType == mimeType)
					return codec;

			return null;
		}
		 private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
		public static void MergeImages()
		{
			string[] imagePaths = Directory.GetFiles(@"C:\Users\Administrator\Desktop\WeiXin");
			if (imagePaths == null || imagePaths.Length == 0) {
				throw new ArgumentException("No image paths provided.");
			}

			int totalWidth = 1000;
			int maxHeight = 0;

			// Get total width and maximum height of all images
			foreach (string imagePath in imagePaths) {
				using (Image image = Image.FromFile(imagePath)) {
					//totalWidth += image.Width;
					if (image.Width > 1000)
						maxHeight += (int)(image.Height / (image.Width / 1000.0f));
					else
						maxHeight += image.Height;
				}
			}

			// Create the final image
			Bitmap finalImage = new Bitmap(totalWidth + 64, maxHeight + 32 * (imagePaths.Length + 1));

			// Use Graphics object to draw each image
			using (Graphics g = Graphics.FromImage(finalImage)) {
				int currentY = 32;
				foreach (string imagePath in imagePaths) {
					using (Image image = Image.FromFile(imagePath)) {
						g.CompositingQuality = CompositingQuality.HighQuality;
						g.InterpolationMode = InterpolationMode.HighQualityBicubic;
						g.SmoothingMode = SmoothingMode.HighQuality;
						g.PixelOffsetMode = PixelOffsetMode.HighQuality;
						var h = (int)(image.Height / (image.Width / 1000.0f));
			
						g.DrawImage(image, new Rectangle(32, currentY, totalWidth, image.Width > 1000 ? h : image.Height));
						currentY += h + 32;
					}
				}
			}
			var f = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "1.jpg");
			//finalImage.Save(f, System.Drawing.Imaging.ImageFormat.Png);
			// Set encoder parameters for quality
			// disable once SuggestUseVarKeywordEvident
			EncoderParameters encoderParams = new EncoderParameters(1);
			encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 80L);
			ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
			// Save the image
			finalImage.Save(f, jpgEncoder, encoderParams);
			finalImage.Dispose();
		}
	}
}
