// src/TIFF/Program.cs
// Written by, Sohail Qayum Malik[sqm@hackers.pk]

using System;
using System.Windows.Media.Imaging;

namespace life.image.processing
{	
    public class TIFF /* Decoder for TIFF */
    {	 		   
	   public TIFF(string path)	   
	   {  	   
	   	   Uri uri = new Uri(path, UriKind.RelativeOrAbsolute);				
		   TiffBitmapDecoder decoder = new TiffBitmapDecoder(uri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
		   
           Console.WriteLine(uri); 		   
	   }	   		   	   
    }
}
