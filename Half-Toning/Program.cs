// Half-Toning/program.cs
// Written by, Sohail Qayum Malik[sqm@hackers.pk]

using System;
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;

namespace half.toning 
{
	static public class HT /* Half Toning */
	{
		public static int Main()
		{
			String[] arguments = Environment.GetCommandLineArgs();
						
			if ( arguments.Length > 3)
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
                         byte[] pixels_ht = new byte[bitmapSource.PixelWidth*bitmapSource.PixelHeight];						 
						 bitmapSource.CopyPixels(pixels, bitmapSource.PixelWidth, 0);
						 halftone(ref pixels, ref pixels_ht, bitmapSource.PixelWidth, bitmapSource.PixelHeight, Convert.ToInt32(arguments[3]));
						 save(arguments[2], ref pixels_ht, bitmapSource.PixelWidth, bitmapSource.PixelHeight, bitmapSource.DpiX, bitmapSource.DpiY, bitmapSource.Format, bitmapSource.Palette);	
					 }
				 }
				 catch(FileNotFoundException ex)
				 {
					 /* :) */
					 Console.WriteLine(ex.ToString());
				 }
			}
			else 
			{
				Console.WriteLine("Example usage, C:\\>ht.Exe source.tif target.tif threshold");
			}
			
			return 0;
		}
		
		static void save(String path, ref byte[] pixels, int width, int height, double DpiX, double DpiY, System.Windows.Media.PixelFormat Format, BitmapPalette Palette)
		{
			BitmapSource image = BitmapSource.Create(width, height, DpiX, DpiY, Format, Palette, pixels, width);
            FileStream stream = new FileStream(path, FileMode.Create);	
			TiffBitmapEncoder encoder = new TiffBitmapEncoder();
			encoder.Frames.Add(BitmapFrame.Create(image));
		    encoder.Save(stream);
		}
		
		/*
            Threshold, Usually set to half the number of available gray shades. For an image with 256 gray shades, set threshold at 128
		*/
		static void halftone(ref byte[] source, ref byte[] target, int width, int height, int threshold)
		{	
		    int m, n, i, j, xx, yy;
			/* 
				Error distribution function. The elements in C must add up to 1. The size of the C and the values at 
                each location were set through trial and error for the best results. 
				This is not an optimal error distribution, there is a room for improvement(as always) 
			*/
			float sum_p, t = (float)0.0;
			/*
                Ep(Propagated Error)
				--------------------
                Sum of the errors propagated to position (m,n) due to prior assignments of generated errors.
                During the very first iteration, there are no previously generated errors so sum of the errors propagated to position (m,n) is zero as well            			
                Eg(Generated Error)
				-------------------
                The total error generated at position (m,n)
            */
			float[,] c = new float[2, 3], Eg = new float[height, width], Ep = new float[height, width];
			c[0,0] = (float)0.0;
			c[0,1] = (float)0.2;
			c[0,2] = (float)0.0;
			c[1,0] = (float)0.6;
			c[1,1] = (float)0.1;
			c[1,2] = (float)0.1;
			
			for (i = 0; i < height; i++)
			{
				for (j = 0; j < width; j++)
				{
					Ep[i,j] = (float)0.0;
					Eg[i,j] = (float)0.0;
				}
			}
			
			for (m = 0; m < height; m++)
			{
				for (n = 0; n < width; n++)
				{	
                    sum_p = (float)0.0; 					
                    for (i = 0; i < 2; i++)
					{
						for (j = 0; j < 3; j++)
						{
							xx = m - i + 1;
							yy = n - j + 1;							
							if (xx < 0)
							    xx = 0; 								
							if (xx >= height)							
								xx = height - 1;
							if (yy < 0)
								yy = 0;							
							if (yy >= width)
								yy = width - 1;	
                            
							sum_p = sum_p + c[i, j]*Eg[xx, yy];							
						} /* Ends loop over j */
					} /* Ends loop over i */	
					
					Ep[m, n] = sum_p;
					t = source[m*n] + Ep[m, n];

					if (t > threshold)
					{					
						Eg[m, n] = t - threshold*2;
                        target[m*n] = 0xff;
					}
					else 
					{
						 Eg[m, n] = t;
						 target[m*n] = 0x00;						
					}
					
				} /* Ends loop over n */
			} /* Ends loop over m */			
		}
	}	
}