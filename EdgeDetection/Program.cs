// EdgeDetection/Program.cs
// Written by, Sohail Qayum Malik[sqm@hackers.pk]

using System;
using System.IO;
using System.Windows.Media.Imaging;

/* 
    C/C++ Uesr Journal archive
	http://collaboration.cmc.ec.gc.ca/science/rpn/biblio/ddj/Website/articles/CUJ/1990/
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
						byte[] pixels = new byte[bitmapSource.PixelWidth*bitmapSource.PixelHeight];                        
						bitmapSource.CopyPixels(pixels, bitmapSource.PixelWidth, 0);
					}
				}
				catch (FileNotFoundException ex)
				{
					/* :) */
                    Console.WriteLine("Some thing exceptional happened");					
				}
                convolution(ref kirsch);			
			}
			else 
			{
				Console.WriteLine("Usage... C:\\> " + arguments[0] + " image.tif");
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
        public static void convolution(ref int[,,] k)
		{
            /*
			    k.Rank returns number of dimension since k is 3 dimensional it'll return 3
                k.GetLength(0 to 2) would then return the size of that specific dimension
                k.GetLength(0) would return 8 and then k.GetLength(1) and k.GetLength(2) would
                return 3
                Perils of Managed Code... internally lot of table lookups should be happening 				
			*/
		}		
	}
}	