// src/main.cs
// Written by, Sohail Qayum Malik[sqm@hackers.pk]

using System;

namespace life.image.processing 
{
	static public class LIP /* Life Image Processing */
	{
		public static int Main()
		{
			int ret = 0;
			
			String[] arguments = Environment.GetCommandLineArgs();
						
			CLI obj = new CLI(ref arguments);
            Uri uri = new Uri("Assets\\current.tif", UriKind.RelativeOrAbsolute);	
			
			//TiffBitmapDecoder decoder = new TiffBitmapDecoder(uri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);

            Console.WriteLine(uri);			
												
			return ret;
		}
	}
}