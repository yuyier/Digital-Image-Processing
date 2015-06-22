// EdgeDetection/Program.cs
// Written by, Sohail Qayum Malik[sqm@hackers.pk]

using System;
using System.IO;
using System.Windows.Media.Imaging;

/* 
    C/C++ Users Journal archive
	http://collaboration.cmc.ec.gc.ca/science/rpn/biblio/ddj/Website/articles/CUJ/1990/9008/faler/faler.htm
	Image Processing in C by, Dwayne Phillips
 */

namespace edge.detection
{
	static public class ED /* Edge Detection */
	{
		public static int Main()
		{	
            String[] arguments = Environment.GetCommandLineArgs(); 	
			
			// Three-dimensional array
            int[,,] kirsch = new int[,,] { 
			                                  {
												  { 5,  5,  5 },
                                                  {-3,  0, -3 },
                                                  {-3, -3, -3 },
											  },
											  
											  {
                                                  {-3,  5,  5},
                                                  {-3,  0,  5},
                                                  {-3, -3, -3},
											  },
											  
											  {
                                                  {-3, -3,  5},
                                                  {-3,  0,  5},
                                                  {-3, -3,  5}, 
											  },
											  
											  {
                                                  {-3, -3, -3},
                                                  {-3,  0,  5},
                                                  {-3,  5,  5},
											  },
											  
											  {
                                                  {-3, -3, -3},
                                                  {-3,  0, -3},
                                                  { 5,  5,  5},
											  },
											  
											  {
                                                  {-3, -3, -3},
                                                  { 5,  0, -3},
                                                  { 5,  5, -3},
											  },
											  
											  {
                                                  { 5, -3, -3},
                                                  { 5,  0, -3},
                                                  { 5, -3, -3}, 
											  },
											  
											  {
                                                 { 5,  5, -3},
                                                 { 5,  0, -3},
                                                 {-3, -3, -3}, 
											  },
										  };
			
            if (arguments.Length >= 2)
			{
				Uri uri = new Uri(arguments[1], UriKind.RelativeOrAbsolute);			
				try 
				{
					TiffBitmapDecoder decoder = new TiffBitmapDecoder(uri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                    BitmapSource bitmapSource = decoder.Frames[0];					
					/* https://msdn.microsoft.com/en-us/library/system.windows.media.pixelformat%28v=vs.110%29.aspx */
					if (bitmapSource.Format.ToString() == "Gray8")
					{
						byte[] image = new byte[bitmapSource.PixelWidth*bitmapSource.PixelHeight];                        
						/* image after convolution */
						byte[] pixels = new byte[bitmapSource.PixelWidth*bitmapSource.PixelHeight];
						bitmapSource.CopyPixels(image, bitmapSource.PixelWidth, 0);
						convolution(ref kirsch, ref image, ref pixels, bitmapSource.PixelWidth, bitmapSource.PixelHeight);
						if (arguments.Length >= 3)
						{
						    save(arguments[2], ref pixels, bitmapSource.PixelWidth, bitmapSource.PixelHeight, bitmapSource.DpiX, bitmapSource.DpiY, bitmapSource.Format, bitmapSource.Palette);
                        }
                        else
						{
							save("convoluted.tif", ref pixels, bitmapSource.PixelWidth, bitmapSource.PixelHeight, bitmapSource.DpiX, bitmapSource.DpiY, bitmapSource.Format, bitmapSource.Palette);
						}
					}
				}
				catch (FileNotFoundException ex)
				{
					/* :) */
                    Console.WriteLine("Some thing exceptional happened");					
				}                
			}
			else 
			{
				Console.WriteLine("Usage... C:\\> " + arguments[0] + " source.tif [target.tif]");
			}
						
			return 0;
		}

		/* I'm a C programmer and array is a pointer to me... */
		/*
		   In mathematics and, in particular, functional analysis, 
		   convolution is a mathematical operation on two functions f and g, 
		   producing a third function that is typically viewed as a modified version of one of the 
		   original functions
		 */
        public static void convolution(ref int[,,] k, ref byte[] image, ref byte[] pixels, int width, int height)
		{
            /*
			    k.Rank returns number of dimension since k is 3 dimensional it'll return 3
                k.GetLength(0 to 2) would then return the size of that specific dimension
                k.GetLength(0) would return 8 and then k.GetLength(1) and k.GetLength(2) would
                return 3
                Perils of Managed Code... internally lot of table lookups should be happening 				
			*/
			
			int i, j;
            for (i = 1; i < height - 1; i++)
			{
				for (j = 1; j < width - 1; j++)
				{
			       int mask;
				   
				   pixels[i*width + j] = 0;
				   
				   for (mask = 0; mask < k.GetLength(0); mask++) 
				   {
					   int row, column, sum = 0; 
					   for (row = -1; row < k.GetLength(1) - 1; row++)
					   {
						   for (column = -1; column < k.GetLength(2) - 1; column++)
						   {
							   sum = sum + image[(i+row)*width + (j+column)]*k[mask, row + 1, column + 1];							   
						   }
					   }
					   
					   if (sum > 255)
					   {
						   sum = 255;
					   }
					   else if (sum < 0)
					   {
						   sum = 0;
					   }
						   					   
					   if (sum > pixels[i*width + j])
					   {
						   pixels[i*width + j] = (byte)sum;  
					   }
				   }									
				}
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