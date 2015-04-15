// src/dll/Program.cs
// Written by, Sohail Qayum Malik[sqm@hackers.pk]

using System;

namespace life.image.processing
{ 	
    public class CLI /* Command Line */
    {
	   public enum Type
       {
          /// <summary>
          /// The application executed successfully.
          /// </summary>
          Help,
          /// <summary>
          /// There was a syntax error in a command line argument.
          /// </summary>
          Version,
       } 
	
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
	   }	   		   	   
    }
}
