// src/TIFF/Program.cs
// Written by, Sohail Qayum Malik[sqm@hackers.pk]

using System;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace life.image.processing
{	
    public sealed class TIFF : IMG /* Decoder for TIFF */
    {

	   TiffBitmapDecoder decoder;
	
	   public TIFF(string path)	   
	   {  	   
	   	   Uri uri = new Uri(path, UriKind.RelativeOrAbsolute);				
		   decoder = new TiffBitmapDecoder(uri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
		   
		   //BitmapSource bitmapSource = decoder.Frames[0];
		   
           //Console.WriteLine(decoder.GetType().ToString()); 		   
		   
           //Console.WriteLine(uri); 		   
	   }

	   public override int PixelWidth()
	   {
		   BitmapSource bitmapSource = decoder.Frames[0];
	       return bitmapSource.PixelWidth;
	   }
	   
	   public override int PixelHeight()
	   {
		   BitmapSource bitmapSource = decoder.Frames[0];
	       return bitmapSource.PixelHeight;		   
	   }	   	   
    }
}
