/*
   js/imagesampler.js
*/

var debug = false;

function image_sampler(image, settings) 
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

function main(image, settings) 
{	
   var imgData = getImagedata(image);   
   if (imgData != false)   
   {
	  var samples = image_sampler(imgData, settings);	  
	  return samples;        
   }
   
   return false;
}

function preloadImage(src, settings)
{	
   var image = new Image();
   image.src = src;
   var samples = false; 
   image.onload = function() 
   {	   
	  if (debug == true)
	  {
	     $('#output').append("Loading image: " + src + "<br>");	  
	  }
	  
      var samples = main(this, settings);      
	  if (samples != false)
	  {
		 output(samples); 
	  }
   }   
}