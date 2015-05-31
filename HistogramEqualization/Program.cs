// HistogramEqualization/program.cs
// Written by, Sohail Qayum Malik[sqm@hackers.pk]

using System;
using System.IO;
using System.Windows.Media.Imaging;

/*
1. calculate histogram
  loop over i ROWS of input image
     loop over j COLS of input image
        k = input_image[i][j]
        hist[k] = hist[k] + 1 // hist.Length max gray levels...
     end loop over j
  end loop over i
  
2. calculate the sum of hist
  loop over i gray levels
     sum = sum + hist[i]
     sum_of_hist[i] = sum
  end loop over i
  
3. transform input image to output image
  area = area of image (ROWS x COLS)
  Dm = number of gray levels in output image
  loop over i ROWS
     loop over j COLS
        k = input_image[i][j]
        out_image[i][j] = (Dm/area) x sum_of_hist[k]
     end loop over j
  end loop over i
*/

namespace histogram.equalization
{
	static public class HE /* Histogram Equalization */
	{
		public static int Main()
		{
			String[] arguments = Environment.GetCommandLineArgs();
						
			if (arguments.Length > 3)
			{ 		
		        Uri uri = new Uri(arguments[1], UriKind.RelativeOrAbsolute);
				/* BitmapCacheOption.Default, If you want to load the same Uri again after modify it, you're going to need to use BitmapCreateOptions.IgnoreImageCache the next time you try to open it */
				try 
				{
					TiffBitmapDecoder decoder = new TiffBitmapDecoder(uri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
					BitmapSource bitmapSource = decoder.Frames[0];
					 /* https://msdn.microsoft.com/en-us/library/system.windows.media.pixelformat%28v=vs.110%29.aspx */
					 if (bitmapSource.Format.ToString() == "Gray8")
					 {
						 byte[] pixels = new byte[bitmapSource.PixelWidth*bitmapSource.PixelHeight];                         
						 bitmapSource.CopyPixels(pixels, bitmapSource.PixelWidth, 0);
						 int[] h = new int[256]; // histogram
						 long[] s = new long[256]; // sum of histogram
						 byte[] transformed = new byte[bitmapSource.PixelWidth*bitmapSource.PixelHeight];
						 
						 calculate_histogram(ref pixels, ref h);
						 /*for (int i = 0; i < h.Length; i++)
						 {
							 Console.Write(i + " = " + h[i] + ", ");
						 }*/
						 calculate_sum_of_histogram(ref h, ref s);
						 transform(ref pixels, ref transformed, ref s, Convert.ToInt32(arguments[3]));
						 save(arguments[2], ref transformed, bitmapSource.PixelWidth, bitmapSource.PixelHeight, bitmapSource.DpiX, bitmapSource.DpiY, bitmapSource.Format, bitmapSource.Palette);
					 }
				}
				catch (FileNotFoundException ex)				
				{
                     /* :) */
					 Console.WriteLine(ex.ToString());					
				}				
			}
			else
			{
				Console.WriteLine(arguments[0] + " source.tif target.tif Dm");
				Console.WriteLine("Dm, it is gray level of target.tif. In an eight bit(Gray8) this is any number between 0 and 255. It is usually set at half the gray-level of the source.tif(in an 8 bit gray level it is set at 128)");
			}
			
			return 0;
		}

        public static void calculate_histogram(ref byte[] pixels, ref int[] h)	
		{
            for (int i = 0; i < pixels.Length; i++)
			{
				h[pixels[i]] = h[pixels[i]] + 1; 
			}				
		}
        public static void calculate_sum_of_histogram(ref int[] h, ref long[] s)		
		{
			long sum = 0;
			
			for (int i = 0; i < s.Length; i++)
			{
				sum = sum + h[i];
				s[i] = sum;
			}			
		}
        public static void transform(ref byte[] pixels,ref byte[] transformed, ref long[] s, int Dm)	
		{			
			for (int i = 0; i < pixels.Length; i++)
			{	
                Console.Write((byte)((((float)/*s.Length*/Dm)/((float)pixels.Length))*s[pixels[i]]) + ", ");		
				transformed[i] = (byte)((((float)/*s.Length*/Dm)/((float)pixels.Length))*s[pixels[i]]);
			}			
		}

		static void save(String path, ref byte[] pixels, int width, int height, double DpiX, double DpiY, System.Windows.Media.PixelFormat Format, BitmapPalette Palette)
		{
			BitmapSource image = BitmapSource.Create(width, height, DpiX, DpiY, Format, Palette, pixels, width);
            FileStream stream = new FileStream(path, FileMode.Create);	
			TiffBitmapEncoder encoder = new TiffBitmapEncoder();
			encoder.Frames.Add(BitmapFrame.Create(image));
		    encoder.Save(stream);
		}		
	}	
}