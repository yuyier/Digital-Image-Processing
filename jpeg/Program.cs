// jpeg/program.cs
// Written by, Sohail Qayum Malik[sqm@hackers.pk]

// https://msdn.microsoft.com/en-us/library/aa970689%28v=vs.110%29.aspx

using System;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace working.with.jpeg
{
	static public class WWJPEG /* Working With JPEG */
	{
		public static int Main()
		{
			String[] arguments = Environment.GetCommandLineArgs();
						
			if (arguments.Length > 2)
			{		
                Uri uri = new Uri(arguments[1], UriKind.RelativeOrAbsolute);														
                try 
				{	
                    /* These three statements are an eye-sore, needed as I know very little about lot of things in this life */				
				    Image image = Image.FromFile(arguments[1], true); 
					int pixelSizeInBytes = Image.GetPixelFormatSize(image.PixelFormat) / 8;
					image.Dispose();
					
					JpegBitmapDecoder decoder = new JpegBitmapDecoder(uri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
					BitmapSource bitmapSource = decoder.Frames[0];
			           							
					Console.WriteLine("DPI-Y = " + bitmapSource.DpiY + ", DPI-X = " + bitmapSource.DpiX);
					Console.WriteLine("PixelHeight = " + bitmapSource.PixelHeight + ", PixelWidth = " + bitmapSource.PixelWidth);					
                    Console.WriteLine("PixelFormat in byte = " + pixelSizeInBytes);					
					
                    byte[] pixels = new byte[bitmapSource.PixelHeight*bitmapSource.PixelWidth*pixelSizeInBytes];                    	                				
                    bitmapSource.CopyPixels(pixels, bitmapSource.PixelWidth*pixelSizeInBytes, 0);
					for (int m = 0; m < bitmapSource.PixelHeight; m++)
					{
						for (int n = 0; n < bitmapSource.PixelWidth; n++)
						{
							//pixels[(m*bitmapSource.PixelWidth*pixelSizeInBytes) + (n*pixelSizeInBytes) + 0] = 0x00; // Blue
							pixels[(m*bitmapSource.PixelWidth*pixelSizeInBytes) + (n*pixelSizeInBytes) + 1] = 0x00; // Green
							pixels[(m*bitmapSource.PixelWidth*pixelSizeInBytes) + (n*pixelSizeInBytes) + 2] = 0x00; // RED							
						}
					}
                    save(arguments[2], ref pixels, bitmapSource.PixelWidth, bitmapSource.PixelHeight, bitmapSource.DpiX, bitmapSource.DpiY, bitmapSource.Format, bitmapSource.Palette, bitmapSource.PixelWidth*pixelSizeInBytes);					
				}
				catch (FileNotFoundException ex)
				 {
					 /* :) */
					 Console.WriteLine(ex.ToString());
				 }
			}
			
			return 0;
		}
		
		static void save(String path, ref byte[] pixels, int width, int height, double DpiX, double DpiY, System.Windows.Media.PixelFormat Format, BitmapPalette Palette, int stride)
		{
			BitmapSource image = BitmapSource.Create(width, height, DpiX, DpiY, Format, Palette, pixels, stride);
            FileStream stream = new FileStream(path, FileMode.Create);	
			JpegBitmapEncoder encoder = new JpegBitmapEncoder();
			encoder.Frames.Add(BitmapFrame.Create(image));
		    encoder.Save(stream);
		}						
	}
}