# How to add support for Modded Biomes

## Using [`ModScan`](Scanners/Mods/ModScan.cs)
1. Create class extending ModScan
2. Create public const string for each popup/card you want to display (this allows the auto scanner to pre-load the asset)
3. override abstract functions
	1.  ScanTowns has first priority for scans, add checks for all your modded towns here. return "" if none found.
	2.  ScanMiniBiomes has second priority, add checks for all your small biomes that may appear within other larger biomes here. return "" if none found.
	3.  ScanBiomes has last priority, add checks for all your standard modded biomes here. return "" if none found.
4. Place all assets for your popups in folder [Assets/Textures](Assets/Textures)
> **Note**
> Make sure the strings and the names of the files match. 
> The string does not need to include the folder name unless you add a folder.
> The filetype does not need to be included (gif are not supported currently, only pngs/jpg)
> No naming convention needed, feel free to use your own

Example: ['ScanCalamity'](Scanners/Mods/ScanCalamity.cs)

## Gif Popups & Sprite Sheets
Gifs are not supported so you will need to create a sprite sheet.
1. The sprite sheet must be vertical (one column)
2. The sheet name must end with '_gif'
3. An image with the same name must exist without the ending '_gif' (eg. 'snow' & 'snow_gif')
	1. This image must be the same dimensions as a frame of the sheet
	2. The animator will scan the dimensions of this image to calculate the frame cuts from the sheet
4. In your ModScan class, return the sheet image (with the '__gif')
5. In your ModScan class, make a public const string for the sheet to pre-load it