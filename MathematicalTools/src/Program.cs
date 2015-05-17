// src/Program.cs
// Written by, Sohail Qayum Malik[sqm@hackers.pk]

using System;
using System.Collections;

namespace life.image.processing 
{
	static public class LIP /* Life Image Processing */
	{
		public static int Main()
		{
			int ret = 0;			
			String[] arguments = Environment.GetCommandLineArgs();						
			CLI cli = new CLI(ref arguments);
						
			if (cli[CLI.Type.Add] && cli.CommonArgc > 2)
			{
				try
				{
			        IMG img1 = new TIFF(arguments[1]); // Like Assets\Current.tif
                    IMG img2 = new TIFF(arguments[2]); // Likewise
                    IMG img3 = (TIFF)img1 + (TIFF)img2;					
				}
				catch (LIPException ex)
				{
				    Console.WriteLine(ex.ToString());
					return ret;
				}
			}
			
			
			
            /*if (cli.CommonArgc > 1) 
			{
				try
				{
 				    img = new TIFF(arguments[1]); // Like Assets\current.tif
                    if (cli[CLI.Type.Add])
					{
					    Console.WriteLine("Given");	
					}						
				    img.save(arguments[1]);
				}
				catch (LIPException ex)				
				{
					Console.WriteLine(ex.ToString());
					// https://msdn.microsoft.com/en-us/library/system.exception.data%28v=vs.110%29.aspx
					Console.WriteLine(ex.Data["original"]);										
					// https://msdn.microsoft.com/en-us/library/system.exception.data%28v=vs.110%29.aspx
					if (ex.Data.Count > 0) 
					{
						foreach (DictionaryEntry de in ex.Data)
                            Console.WriteLine("    Key: {0,-20}      Value: {1}", "'" + de.Key.ToString() + "'", de.Value);
					}
					else
					{
						Console.WriteLine("Nop");
					}
					
					return ret;
				}
				
				//Console.WriteLine(img.PixelWidth());
				//Console.WriteLine(img.PixelHeight());
			}*/				
									                         			
			return ret;
		}
	}
}