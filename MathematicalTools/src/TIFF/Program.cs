// src/TIFF/Program.cs
// Written by, Sohail Qayum Malik[sqm@hackers.pk]

using System;
// using System.Drawing;
using System.IO;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

// https://msdn.microsoft.com/en-us/library/system.windows.media.imaging.bitmapdecoder(v=vs.110).aspx
// https://msdn.microsoft.com/en-us/library/system.drawing.image(v=vs.110).aspx
// https://msdn.microsoft.com/en-us/library/aa969817(v=vs.110).aspx
namespace life.image.processing
{	
    public sealed class TIFF : IMG /* Decoder for TIFF */
    {
	   TiffBitmapDecoder decoder;
	
	   public TIFF(string path)	   
	   {  	   
	   	   Uri uri = new Uri(path, UriKind.RelativeOrAbsolute);
           try 
		   {
		       decoder = new TiffBitmapDecoder(uri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
		   }
		   catch (FileNotFoundException ex)
		   {
			   LIPException e = new LIPException("TiffBitmapDecoder() failed at uri = " + uri.ToString(), ex.ToString());
			   //throw new LIPException(ex.ToString());	
			   throw e;
		   }		   
		   //BitmapSource bitmapSource = decoder.Frames[0];
		   
           //Console.WriteLine(decoder.GetType().ToString()); 		   
		   
           //Console.WriteLine(uri); 		   
	   }
	   
	   // Smaller will be added to larger
	   // http://stackoverflow.com/questions/1176910/finding-specific-pixel-colors-of-a-bitmapimage
	   // http://w3facility.org/question/c-how-to-encode-int-array-to-tiff-image/
	   public static TIFF operator +(TIFF imgA, TIFF imgB)
	   {	
           int width = imgA.PixelWidth() - imgB.PixelWidth(); 	   
		   int height = imgA.PixelHeight() - imgB.PixelHeight();
		   BitmapSource bmsA = imgA.getBitmapSource();
		   BitmapSource bmsB = imgB.getBitmapSource();		   
		   // You need .NET 4.5 or later
		   //WritableBitmap wbitmap = new WritableBitmap(bmsA);
		   
		   Console.WriteLine(BitmapPalettes.Gray256);

		   /*
		       When width less than zero take the width of imgA else take the width of imgB
			   When height less than zero take the height of imgA else take the height of imgB
		   */		   
		   if (width <= 0)
		   {
			   width = imgA.PixelWidth();
		   }
		   else 
		   {
			   width = imgB.PixelWidth();
		   }
		   
		   if (height <= 0)
		   {
			   height = imgA.PixelHeight();
		   }
		   else
		   {
			   height = imgB.PixelHeight();
		   }
		   
		   byte[] imgApixels = new byte[width*height];
		   byte[] imgBpixels = new byte[width*height];
		   bmsA.CopyPixels(imgApixels, width*height, 0);
		   bmsB.CopyPixels(imgBpixels, width*height, 0);
		   
		   Console.WriteLine(BitmapPalettes.Gray256);
		  
           /* Things are not working here.... It is bmsA.Palette which is causing all the problems... */	
           /* Uncomment the following statement and then run it... */
           /* E:\Image-Processing>LIP.Exe Assets\Current.tif Assets\Current.tif /_a */		  
		   // BitmapSource image = BitmapSource.Create(width, height, bmsA.DpiX, bmsA.DpiY, bmsA.Format, bmsA.Palette, imgApixels, width);
		   
		   /*
		   http://stackoverflow.com/questions/25755800/draw-a-colour-bitmap-as-greyscale-in-wpf
		   FormatConvertedBitmap(bitmap, PixelFormats.Gray8, BitmapPalettes.Gray256, 0.0)
		   https://msdn.microsoft.com/en-us/library/system.windows.media.imaging.tiffbitmapdecoder%28v=vs.90%29.aspx
		      int width = 128;
              int height = width;
              int stride = width / 8;
              byte[] pixels = new byte[height * stride];

              // Define the image palette
              BitmapPalette myPalette = BitmapPalettes.WebPalette;

              // Creates a new empty image with the pre-defined palette

              BitmapSource image = BitmapSource.Create(
              width,
              height,
              96, //The horizontal dots per inch (dpi) of the bitmap. A property of BitmapSource Class
              96, //The vertical dots per inch (dpi) of the bitmap. A property of BitmapSource Class
              PixelFormats.Indexed1, // BitmapSource.Format Property 
              myPalette, 
              pixels,
              stride);//the stride width (also called scan width)

              FileStream stream = new FileStream("new.tif", FileMode.Create);
              TiffBitmapEncoder encoder = new TiffBitmapEncoder();
              TextBlock myTextBlock = new TextBlock();
              myTextBlock.Text = "Codec Author is: " + encoder.CodecInfo.Author.ToString();
              encoder.Compression = TiffCompressOption.Zip;
              encoder.Frames.Add(BitmapFrame.Create(image));
              encoder.Save(stream);
		  */
		   
		   return imgA;		   
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
	   
	   public override void save(string f)
	   {	
           FileStream stream = new FileStream(f + ".tif", FileMode.Create);	
           TiffBitmapEncoder encoder = new TiffBitmapEncoder();	
           encoder.Frames.Add(decoder.Frames[0]);
           encoder.Save(stream);		   
	   }
	   
	   BitmapSource getBitmapSource()
	   {
		   return decoder.Frames[0];
	   }
    }
}
