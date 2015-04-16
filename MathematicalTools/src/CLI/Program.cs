// src/CLI/Program.cs
// Written by, Sohail Qayum Malik[sqm@hackers.pk]

using System;

namespace life.image.processing
{ 	
    /* TODO, should be able to process multiple instances of same CL option */
    public sealed class CLI /* Command Line */
    {
	    public enum Type
        {
           /// <summary>
           /// It displays the help screen.
           /// </summary>
           Help,
           /// <summary>
           /// It displays the version number.
           /// </summary>
           Version,
        }
	   
	    public struct args
	    {
		   public string	name;
		   public string	description;
		   public Type		type;
           public int		index;
		   public int		argc;
	    }
	   	
        /* TODO, Lot of redundancy, remove it */		
	    readonly args[] tokens = new args[] {
		   new args{name = "/_h", description = "It displays the help screen", type = Type.Help, index = 0, argc = 0},
		   new args{name = "/-h", description = "It displays the help screen", type = Type.Help, index = 0, argc = 0},
		   new args{name = "/_v", description = "It displays the version number", type = Type.Version, index = 0, argc = 0},
		   new args{name = "/-v", description = "It displays the version number", type = Type.Version, index = 0, argc = 0},
	    };

        int commonArgc;		
		   	
	    public delegate bool del(string arg, args token);
	   
	    public CLI(ref String[] arguments)	   
	    {  
           int argc = arguments.Length;	   
		   del tryParseOption = (arg, token) => {
			  
			   bool ret = false; 
			  
			   if (arg.Equals(token.name, StringComparison.Ordinal)) 
			   {
				   ret = true; 
			   }
			  
			   return ret;   
	       };
		   
		   commonArgc = 0;
          
           for (int i = 0; i < tokens.Length; i++) 
		   {
			   for (int j = 0; j < arguments.Length; j++)
			   {
				   if (tryParseOption(arguments[j], tokens[i])) 
				   {					   
					   if (commonArgc == 0)
					   {
						   commonArgc = j;
					   }
					   else if (j < commonArgc)
					   {
						   commonArgc = j;
					   }
					   
				       tokens[i].index = j;  				   
                       int k = 0;
				       bool flag = false;
                       while (!flag && k < tokens.Length)
				       {					
                           int l = j + 1;
					       while (!flag && l < arguments.Length)
					       {
						       if (tryParseOption(arguments[l], tokens[k]))
						       {	
                                   tokens[i].argc = l - tokens[i].index;	
							       flag = true; 
						       }							 
						       l++;  
					       }						  
					       k++; 
				       }
              
				       if (flag == false)
				       {
					       tokens[i].argc = arguments.Length - tokens[i].index;					  
				       }
				   }
			   }				 
		   }
		   
		   if (commonArgc == 0)
		   {
			   commonArgc = argc; 
		   }
		  
		   /*for (int i = 0; i < tokens.Length; i++)
		   {
			   if (tokens[i].index > 0)
			   {
				   Console.WriteLine("Received");
                   Console.WriteLine(tokens[i].name);
                   Console.WriteLine(tokens[i].index);
                   Console.WriteLine(tokens[i].argc);
			   }				 
		   }*/		    		  
	    }

        public int CommonArgc 
		{
	        get {return commonArgc;}		
		} 		
    }
}
