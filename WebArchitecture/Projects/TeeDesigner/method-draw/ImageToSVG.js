/**
 * Created by CharlieBarber on 5/3/16.
 */
function imgToSVG(path)
{
    ImageTracer.loadImage(
        path,
        function(canvas){

            // Getting ImageData from canvas with the helper function getImgdata().
            var imgdata = ImageTracer.getImgdata( canvas );

            // Synchronous tracing to SVG string
           return ImageTracer.imagedataToSVG( imgdata, { scale:5 } );
        }
    );
}

