//Declare Global Variables
var imageData;
var fillLock = false;
var oldRGBArray;
var newRGBArray;

/// <summary>
/// Shows image preview
/// </summary>
function showModal(imgURI) {
    var modal = document.getElementById("myModal");
    modal.style.display = "block";
    var modalImg = document.getElementById("img01");
    modalImg.src = imgURI;
    return imgURI;
}

/// <summary>
/// Hides image preview
/// </summary>
function hideModal() {
    var modal = document.getElementById("myModal");
    modal.style.display = "none";
}

/// <summary>
/// Gets the color at the given pixel position
/// </summary>
function getColor(mouseX, mouseY) {
    var canvas = document.getElementById("canvas").children[0];
    var context = canvas.getContext('2d');
    var selectedPixel = context.getImageData(mouseX, mouseY, 1, 1);
    return ("rgb(" + selectedPixel.data[0] + ", " + selectedPixel.data[1] + ", " + selectedPixel.data[2] + ")");
}

/// <summary>
/// Flood fill from the position given
/// </summary>
function bucketFill(oldRGB, newRGB, mouseX, mouseY) {

    if (oldRGB == newRGB) return;
    if (fillLock) return;
    fillLock = true;

    var canvas = document.getElementById("canvas").children[0];
    var context = canvas.getContext('2d');
    context.fillStyle = newRGB;
    imageData = context.getImageData(0, 0, canvas.width, canvas.height);

    newRGBArray = splitRGBToArray(newRGB);
    oldRGBArray = splitRGBToArray(oldRGB);

    pixelStack = [[mouseX, mouseY]];

    while (pixelStack.length) {
        var newPos;
        var x;//x position of the current pixel
        var y;//y position of the current pixel
        var pixelPos;//array position of the current pixel
        var reachLeft;//bool, whether we should fill left pixels
        var reachRight;//bool, whether we should fill right pixels
        newPos = pixelStack.pop();
        x = newPos[0];
        y = newPos[1];

        pixelPos = (y * canvas.width + x) * 4;//get array position of the current pixel
        while (y-- >= 0 && matchStartColor(pixelPos)) {//move upwards through pixel to edge of shape
            pixelPos -= canvas.width * 4;
        }
        pixelPos += canvas.width * 4;//move down back into the shape
        ++y;
        reachLeft = false;
        reachRight = false;
        while (y++ < canvas.height - 1 && matchStartColor(pixelPos)) {//loop so long as we can move down and are within the shape

            //set the color of the current pixel
            imageData.data[pixelPos] = newRGBArray[0];
            imageData.data[pixelPos + 1] = newRGBArray[1];
            imageData.data[pixelPos + 2] = newRGBArray[2];
            imageData.data[pixelPos + 3] = 255;

            if (x > 0) {
                if (matchStartColor(pixelPos - 4)) {//if the left pixel is in our shape
                    if (!reachLeft) {//and we haven't currently set left to fill
                        pixelStack.push([x - 1, y]);
                        reachLeft = true;
                    }
                }
                else if (reachLeft) {//if we've reached a new left section that needs to be filled
                    reachLeft = false;
                }
            }

            if (x < canvas.width - 1) {//same as above but for the right
                if (matchStartColor(pixelPos + 4)) {
                    if (!reachRight) {
                        pixelStack.push([x + 1, y]);
                        reachRight = true;
                    }
                }
                else if (reachRight) {
                    reachRight = false;
                }
            }

            pixelPos += canvas.width * 4;//go down one pixel
        }
    }

    context.putImageData(imageData, 0, 0);
    fillLock = false;
}

/// <summary>
/// Checks if the pixel at the passed position matches the old color
/// </summary>
function matchStartColor(pixelPos) {
    var r = imageData.data[pixelPos];
    var g = imageData.data[pixelPos + 1];
    var b = imageData.data[pixelPos + 2];

    return (r == oldRGBArray[0] && g == oldRGBArray[1] && b == oldRGBArray[2]);
}

/// <summary>
/// Splits an RGB string into an array
/// </summary>
function splitRGBToArray(rgb) {
    return rgb.split("(")[1].split(")")[0].split(",");
}

/// <summary>
/// Rotates the canvas 90 degrees, clockwise if clockwise is true,
/// else counter-clockwise
/// </summary>
function rotateCanvas(clockwise) {

    var canvas = document.getElementById("canvas").children[0];
    var context = canvas.getContext('2d');
    var canvasImage = new Image();
    canvasImage.src = canvas.toDataURL();//copy the current contents of the canvas

    canvasImage.onload = function () {

        context.save();
        if (clockwise) {
            context.translate(canvas.width, 0);
            context.rotate(90 * Math.PI / 180);
        }
        else {
            context.translate(0, canvas.height);
            context.rotate(-90 * Math.PI / 180);
        }
        context.clearRect(0, 0, canvas.width, canvas.height);//clear canvas before drawing new rotated image
        context.drawImage(canvasImage, 0, 0);
        context.restore();
    }

}

/// <summary>
/// Flips the canvas, horizontally if horizontal is true,
/// else vertically
/// </summary>
function flipCanvas(horizontal) {
    var canvas = document.getElementById("canvas").children[0];
    var context = canvas.getContext('2d');
    var canvasImage = new Image();
    canvasImage.src = canvas.toDataURL();//copy the current contents of the canvas

    canvasImage.onload = function () {

        context.save();
        if (horizontal) {
            context.translate(canvas.width, 0);
            context.scale(-1, 1);
        }
        else {
            context.translate(0, canvas.height);
            context.scale(1, -1);
        }
        context.clearRect(0, 0, canvas.width, canvas.height);//clear canvas before drawing new flipped image
        context.drawImage(canvasImage, 0, 0);
        context.restore();
    }
}

/// <summary>
/// Draws the passed stamp at the passed location on the canvas
/// </summary>
function placeStamp(mouseX, mouseY, stamp, scale) {
    var canvas = document.getElementById("canvas").children[0];
    var context = canvas.getContext('2d');
    var stampImage = new Image();
    stampImage.src = "/images/" + stamp + ".webp";
    stampImage.onload = function () {
        var scalePercent = scale / 100;
        var newWidth = stampImage.width * scalePercent;
        var newHeight = stampImage.height * scalePercent;
        context.drawImage(stampImage, mouseX - (newWidth/2), mouseY - (newHeight/2), newWidth, newHeight);
    }

}