// src/IMG/Program.cs
// Written by, Sohail Qayum Malik[sqm@hackers.pk]

using System;

namespace life.image.processing
{	

   public class LIPException: Exception
   {
      public LIPException()
      {
      }

      public LIPException(string message, string original) : base(message)
      {
		  // https://msdn.microsoft.com/en-us/library/system.exception.data%28v=vs.110%29.aspx
		  this.Data["original"] = "============= Original Message =============\n" + original + "\n============= Original Message Ended =============\n";
      }

      public LIPException(string message, Exception inner) : base(message, inner)
      {
      }
   }

    public abstract class IMG /* Base class Image */
    {
	   public abstract void save(string f);	
	   public abstract int PixelWidth();
       public abstract int PixelHeight(); 	   
	   // Not possible
       //public static abstract IMG operator +(IMG imgA, IMG imgB);       
    }
}
