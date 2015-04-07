// src/dll/Program.cs
// Written by, Sohail Qayum Malik[sqm@hackers.pk]

using System;
using System.Windows.Media.Imaging;

namespace life.image.processing
{	
    public class CLI /* Command Line */
    {	 	
	   public delegate bool del(string arg);
	   
	   public CLI(ref String[] arguments)	   
	   {  
           int argc = arguments.Length, i = 1;	   
		   del tryParseOption = arg => {
			  
			   bool ret = false; 
			  
			   if (arg.Equals("/_v", StringComparison.Ordinal) || arg.Equals("-_v", StringComparison.Ordinal)) 
			   {
				   // Verbose 			
				   ret = true; 
			   }
			   else if (arg.Equals("/_h", StringComparison.Ordinal) || arg.Equals("-_h", StringComparison.Ordinal))
			   {
				   // Help
				   ret = true;
			   }
			  
			   return ret;   
		  };
          	  
		  while (argc > 1)
		  {
			  tryParseOption(arguments[i]); 
			  i++;
			  argc--; 
		  }
		  
		  Uri uri = new Uri("Assets\\current.tif", UriKind.RelativeOrAbsolute);				
		  TiffBitmapDecoder decoder = new TiffBitmapDecoder(uri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
	   }	   		   	   
    }
}
