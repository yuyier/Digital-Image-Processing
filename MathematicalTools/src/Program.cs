// src/Program.cs
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
			CLI cli = new CLI(ref arguments);
			
            if (cli.CommonArgc > 1) 
			{
 				IMG img = new TIFF(arguments[1]); // Like Assets\current.tif
			}				
									                         			
			return ret;
		}
	}
}