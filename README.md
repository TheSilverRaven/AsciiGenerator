# AsciiGenerator
A quick and simple Image to Ascii converter console application.

## Use in your own project
Create a c# console application and paste in these files. Build/Debug build and tada! done :>

## How to use
Drag and drop image file onto the application to convert it, just run program to edit configs.

## Available configs: 
Chars: The list of characters the ascii text is generated from. Ranges from brightest character to darkest. Default is ` .:-=+#%`

Inverted: Whether or not to invert the value of the input image. Default is `true`

Curve Filter (optional): Applies a filter curve to the brightness value. Default is `cubicOut`. Possible values are `cubicIn` `cubicOut` `cubicInOut` `quintIn` `quintOut` `quintInOut` and non practical values `bounceIn` `bounceIn` and `bounceInOut`. 
See [Easing Functions Cheatsheet](https://easings.net/) for more details.
