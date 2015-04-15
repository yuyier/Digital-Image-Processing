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
			
			CLI.Type h = CLI.Type.Help;
			
			TIFF t = new TIFF("Assets\\current.tif");
            															
			return ret;
		}
	}
}