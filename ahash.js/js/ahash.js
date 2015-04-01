/*
   js/ahash.js  
*/

var debug = false;

/*
   Step 1. Reduce size...   
*/
function reduce_size(image, settings) 
{
   var samples = [];   
   for (var r = 0; r < settings.rows; r++) 
   {
      samples[r] = [];         
      for (var c = 0; c < settings.columns; c++)
	  {               
         samples[r][c] = {red: 0, green: 0, blue: 0};
      }
   }

   var pixels_per_sample = {            
      horizontally: Math.floor((image.width - 2 * settings.bleed) / settings.columns),         
      vertically: Math.floor((image.height - 2 * settings.bleed) / settings.rows)
   };
   
   for (var r = 0; r < settings.rows; r++) 
   {
	  for (var c = 0; c < settings.columns; c++)
	  {
		 var row = (settings.bleed*image.width + r*pixels_per_sample.vertically*image.width)*4;
         var column = (settings.bleed*pixels_per_sample.horizontally + c*pixels_per_sample.horizontally)*4;		 
		 
		 var pixel_count = 0;
         var rgb = {red: 0, green: 0, blue: 0};
		 
		 /* Displacement */
		 for (var pix = 0; pix < pixels_per_sample.horizontally; pix+=settings.accuracy)
		 {
			rgb.red += image.data[row + column + pix*4];
            rgb.green += image.data[row + column + pix*4 + 1];
            rgb.blue += image.data[row + column + pix*4 + 2];
			
            pixel_count++;                   									
		 }
      		 
		 samples[r][c] = {
            red: ~~(rgb.red / pixel_count),
            green: ~~(rgb.green / pixel_count),
            blue: ~~(rgb.blue / pixel_count)
         };
	  }		   
   }         
   return samples;
}

/*
   Step 2. Reduce color...
   The image is reduced to a grayscale just to further simplify the number of computations
*/
function reduce_color(samples, settings)
{
	var reduced = new Float64Array(settings.rows * settings.columns); 
	
	for (var r = 0; r < settings.rows; r++) 
	{        
	   for (var c = 0; c < settings.columns; c++) 
	   {            		  
		  reduced[settings.rows*r + c] = 0.299*samples[r][c].red + 0.587*samples[r][c].green + 0.114*samples[r][c].blue;
	   }
	}	
	
	return reduced;
}

/*
    Step 3. Average the colors...
    Compute the mean value of colors	
*/
function arithmatic_mean(reduced, settings)
{  
   var summed = 0.0;  	
   
   for (var x = 0; x < settings.rows*settings.columns; x++)
   {
	  summed = summed + reduced[x]; 
   }

   return (summed/(settings.rows*settings.columns));   
}

/*
    Step 4 and 5. Compute the bits and construct hash
	Each bit is simply set based on whether the color value is above or below the mean	
*/
function construct_hash(reduced, mean, settings)
{      
   var hash = '';
   
   for (var x = 0; x < settings.rows*settings.columns; x++)	
   {
	   if (reduced[x] > mean)
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
function getImagedata(image)
{
   var canvas = document.createElement('canvas');
   var context = canvas.getContext && canvas.getContext('2d'); 	
   var height = canvas.height = image.naturalHeight || image.offsetHeight || image.height;
   var width = canvas.width = image.naturalWidth || image.offsetWidth || image.width;      
   // draw image
   try 
   {
      context.drawImage(image, 0, 0);
	  var data = context.getImageData(0, 0, width, height);
	  return data;
   }
   catch (e)
   {
	  return false;
   }      
}


function main(image) 
{
   var hash = 0.0;	
   var imgData = getImagedata(image);   
   if (imgData != false)
   {
	  var defaults = {
         rows: 8,     // the number of horizontal samples, sampled rows
         columns: 8,  // the number of vertical samples, sampled columns
         accuracy: 5, // sample every accuracy'th pixel for better performance
         bleed: 5,    // distance to image border to avoid faulty analysing
         async: true  // execute asynchronous to avoid blocking the application
      }; 
	  var samples = reduce_size(imgData, defaults);	  
	  
	  var reduced = reduce_color(samples, defaults);	  
	  
	  var mean = arithmatic_mean(reduced, defaults);	  
	  	  
	  hash = construct_hash(reduced, mean, defaults);	  
   }
   
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
	  
      var hash = main(this);
	  
	  if (debug == true)
	  {
	     $('#output').append("Hash = <b>" + hash + "</b><br>");
	  }	  
   }  
}