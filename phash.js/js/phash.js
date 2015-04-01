/*
   js/phash.js 
*/

var debug = false;

/* 
  Step 1. Reduce size 
  -------------------
* Like Average Hash, pHash starts with a small image. 
* However, the image is larger than 8x8; 32x32 is a good size. 
* This is really done to simplify the DCT computation and not 
* because it is needed to reduce the high frequencies.
*/
function reduce_size(image, rows /* aka width */, columns /* aka height */)
{
   var canvas = document.createElement('canvas');
   if (debug == true)
   {
	  $('#output').append('Image cropped to ' + columns + "x" + rows +"<br>");
      document.body.appendChild(canvas);
   }
   var context = canvas.getContext && canvas.getContext('2d'); 	
   var height = rows; /*canvas.height = image.naturalHeight || image.offsetHeight || image.height*/ 
   var width = columns; /* canvas.width = image.naturalWidth || image.offsetWidth || image.width */      
   // draw image
   try 
   {
      context.drawImage(image, 0, 0, width, height);
	  var data = context.getImageData(0, 0, width, height);
	  return data;
   }
   catch (e)
   {
	  return false;
   }      
}

/* 
  Step 2. Reduce color
  --------------------    
* The image is reduced to a grayscale just to further simplify 
* the number of computations
*/
function reduce_color(data)
{    
   if (debug == true)
   {
	  $('#output').append("reducing color of " + data.width + "x" + data.height + " image<br>");  
   }

   var vals = new Float64Array(data.width*data.height);   
   
   for (var row = 0; row < data.height; row++)
   {        
      for (var column = 0; column < data.width; column++)
	  {           
		 var base = 4*(data.width*row + column);
		 vals[data.width * row + column] = 0.299 * data.data[base] + 0.587 * data.data[base + 1] + 0.114 * data.data[base + 2];		 
	  }
   }   
   
   return vals;
}

/*  
   Step 3. Compute the DCT(Discrete Cosine Transform)
   --------------------------------------------------
   * The DCT separates the image into a collection of frequencies 
   * and scalars. While JPEG uses an 8x8 DCT, this algorithm uses 
   * a 32x32 DCT.
*/
   /*
      The general equation for a 2D (N by M image)...
	  http://www.cs.cf.ac.uk/Dave/Multimedia/node231.html 	
   */		  
/* size, vals */  
function applyDCT2(N, f)
{
   // initialize coefficients
   var A = new Float64Array(N);    
   /*
      The general equation for a 2D (N by M image)...
	  http://www.cs.cf.ac.uk/Dave/Multimedia/node231.html
      A(0) = 1/sqrt(2)
	  A(i) = 1 where i goes from 1 to N - 1 
   */   
   for (var i = 1; i < N; i++) 
   {
	  A[i] = 1;
   }
   A[0] = 1 / Math.sqrt(2);
      		
   // output goes here, this algorithm uses 32x32 DCT. Sizeof(f)=32x32        
   var F = new Float64Array(N * N);
   
   for (var u = 0; u < N; u++)
   {
	  for (var v = 0; v < N; v++)
	  {
		 /* SUM from i = 0 to i = N - 1 */  
		 var sumi = 0; 
		 for (var i = 0; i < N; i++)
		 {
			/* SUM from j = 0 to j = M - 1 */ 
			var sumj = 0; 
			for (var j = 0; j < N; j++)
			{			   
			  sumj = sumj + A[i]* A[j] * Math.cos(((Math.PI*u)/(2*N))*(2*i + 1)) * Math.cos(((Math.PI*v)/(2*N))*(2*j + 1)) * f[N*i + j];			   	
			}
            sumi = sumi + sumj;			
		 }
         F[N*u + v] = sumi;		 
	  }       	  
   }
   
   return F
}

/* 
   Step 4. Reduce the DCT
   ----------------------
   * This is the magic step. While the DCT is 32x32, just keep the 
   * top-left 8x8. Those represent the lowest frequencies in the 
   * picture.
*/
function reduce_dct_to_8x8(F)
{
	var reduced_dct = [64];
	for(var row = 0; row < 8; row++){
		for(var column = 0; column < 8; column++){
			/* pick the first 8 pixels */
			reduced_dct[8 * row + column] = F[8 * (row + 1) + (column + 1)];
		}
	}
	return reduced_dct;
}

/* Step 5 */
/* Bubble sort */
function bubble_sort(foo) {
	
   var baz = [foo.length];
   for (var i = 0; i < foo.length; i++)
   {
	  baz[i] = foo[i]; 
   }	   
   for (var i = 0; i < baz.length - 1; i++)
   {
	  for (var j = 0; j < baz.length - i - 1; j++)
	  {
		 if (baz[j] > baz[j + 1]) 
		 {
			var bar = baz[j + 1];
			baz[j + 1] = baz[j];
			baz[j] = bar;            			
		 }			 
	  }		  
   }	   

   return baz;	
}
/* Get median, we know array has  which is an even number */
function get_median(reduced_dct)
{
   return (reduced_dct[32] + reduced_dct[33])/2;	
}

/* Step 6 */
function construct_hash(reduced_dct, median)
{      
   var hash = '';
   
   for (var x = 0; x < reduced_dct.length; x++)	
   {
	   if (reduced_dct[x] > median)
	   {
		  hash = hash + ' ' + '1'; 		  
	   }
	   else 
	   {
		  hash = hash + ' ' + '0'; 
	   }
   }
   
   return hash;
}




/* ------------------- */
/* Helper functions... */
/* ------------------- */

function from_data_to_rgb(data, width, height)
{
   var samples = [];   
   for (var row = 0; row < height; row++) 
   {
      samples[row] = [];         
      for (var column = 0; column < width; column++)
	  {               
         samples[row][column] = {red: 0, green: 0, blue: 0};
      }
   } 
   
   for (var row = 0; row < height; row++)
   {
	  for (var column = 0; column < width; column++)
	  {
		 samples[row][column].red = data[width*row*4 + column*4 + 0];
         samples[row][column].green = data[width*row*4 + column*4 + 1];
         samples[row][column].blue = data[width*row*4 + column*4 + 2]; 
	  }		  
   }
   
   return samples;   
}

function main(image) 
{
   var imageData = reduce_size(image, 32, 32); 
   var data = reduce_color(imageData);      
   if (debug==true)
   {
      var samples = from_data_to_rgb(imageData.data, imageData.width, imageData.height);        
      output(samples);
   }
   var data = reduce_color(imageData);
   if (debug==true)
   {
	  $('#output').append("<br>"); 
   }
   var dct = applyDCT2(32, data); 
   var reduced_dct = reduce_dct_to_8x8(dct);
   var reduced_dct_sorted = bubble_sort(reduced_dct);
   var median = get_median(reduced_dct_sorted);   
   var hash = construct_hash(reduced_dct, median);
   
   $('#output').append(hash + "<br>");
   
   return hash;   
}

function preloadImage(src)
{	
   var image = new Image();
   image.src = src;
   image.onload = function() 
   {	   
	  if (debug == true)
	  {
	     $('#output').append("Loading image: " + src + "<br>");	  
	  }
	  
      return main(this);	  
   }  
}