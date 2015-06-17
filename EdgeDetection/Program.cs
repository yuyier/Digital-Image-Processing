// EdgeDetection/program.cs
// Written by, Sohail Qayum Malik[sqm@hackers.pk]

using System;

namespace edge.detection
{
	static public class ED /* Edge Detection */
	{
		public static int Main()
		{
			
			// Three-dimensional array. 
            int[,,] kirsch = new int[,,] { 
			                                  {
												  { 5,  5,  5 },
                                                  {-3,  0, -3 },
                                                  {-3, -3, -3 },
											  },
											  
											  {
                                                  {-3,  5,  5},
                                                  {-3,  0,  5},
                                                  {-3, -3, -3},
											  },
											  
											  {
                                                  {-3, -3,  5},
                                                  {-3,  0,  5},
                                                  {-3, -3,  5}, 
											  },
											  
											  {
                                                  {-3, -3, -3},
                                                  {-3,  0,  5},
                                                  {-3,  5,  5},
											  },
											  
											  {
                                                  {-3, -3, -3},
                                                  {-3,  0, -3},
                                                  { 5,  5,  5},
											  },
											  
											  {
                                                  {-3, -3, -3},
                                                  { 5,  0, -3},
                                                  { 5,  5, -3},
											  },
											  
											  {
                                                  { 5, -3, -3},
                                                  { 5,  0, -3},
                                                  { 5, -3, -3}, 
											  },
											  
											  {
                                                 { 5,  5, -3},
                                                 { 5,  0, -3},
                                                 {-3, -3, -3}, 
											  },
										  };
										  
            Console.WriteLine(kirsch.Length);										  
			
			
			return 0;
		}			
	}
}	