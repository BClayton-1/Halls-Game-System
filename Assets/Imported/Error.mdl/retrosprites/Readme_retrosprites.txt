//-------------------------------------------------------------------------
// Retrosprite Shader v2.0
//
// By Error.mdl
//-------------------------------------------------------------------------

CHANGELOG
v2.0
-Changed to use texture arrays for simplicity and efficiency
-Changed to allow the sprite to have an arbitrary number of directions
-Changed to allow partially unfilled sprite sheets
-fixed lighting calculations not including ambient light
-added versions for particle systems
v1.0
-initial version 

0. PURPOSE
This shader imitates the directional sprites used in many early to mid 90's
first person games like Doom, Daggerfall, Marathon, Hexen, etc. Rather than
use 3d models for enemies, these games used 2d billboards which would change
their image based on the relative postions of the viewer and the enemy. For
example, if you stand behind an enemy the image on the billboard will show
their back, and as you move around them the image will change to show their
side and front from various angles.

1. USAGE

1a. CREATING SPRITE SHEETS
If your sprite is animated you will first need to assemble sprite sheets from
individual sprites. Sprites for each direction go in separate sheets. The
shader reads frames from the sheet going left to right through each row and
reads rows top to bottom, just like text. Unlike the previous version of this
shader, you may leave empty spaces at the end of the sheet. Every sheet must
have exactly the same dimensions and number of sprites!

1b. CREATING TEXTURE ARRAYS
This shader does not use normal textures. It instead uses an object called
a Texture2DArray, which is essentially just a bunch of textures with identical
dimensions and properties bundled together in a single object. Using a
Texture2DArray massively simplifies and increases the efficiency of this
shader, but requires the user to go through a couple of extra steps to create
an array from their source textures using the included scripts.

Once you have your sprite sheets, right click in your project view and click
Create>Texture Lists>2D Texture list. Name the texture list object. In the
inspector change the size of Tex Array to how many directions you have and
add the texture sheets to the array. Element 0 should be the front-facing
sprite sheet, and each successive element should move counter-clockwise
around the sprite.

Now go to tools/Create Texture Array. Drop the texture list you just made into
the corresponding box and click "Create Array". Assuming all your sprite
sheets have the same dimensions and same properties, you'll be prompted to
save the texture array.

1c. APPLYING THE SHADER
You should apply a material using the shader to a Z-facing quad, either
sprite.fbx included with this shader or unity's quad primitive (which faces
-z so the texture will be mirrored). The parameters for the shader are mostly
self-explanatory. Cutout versions have the Alpha to coverage checkbox, which
enables a few degrees of transparency while properly depth sorting.

Non-Particle versions have four parameters used for sprite-sheet animations.
The first number is how many sprites you have in each row (ie how many columns
there are), the second is how many rows you have, the third is the total
number of frames (columns times rows, minus the number of empty spaces you
have at the end of the sprite sheet) and the fourth is how many frames play
per second. If you want to manually animate the sprite as a part of an
animation, set the fourth number to 0 and instead change the manual frame
number property in the animation. Particle verisons lack these parameters,
and instead rely on the particle system's sprite sheet animation feature.

In order to get the shader to work properly with particle systems, you must
change several settings in the render tab. Render Mode should be billboard
(default). Render alignment should be world. Custom vertex streams should be
enabled with these streams, in order: Position, Color, UV, Center, Velocity.
Light Probes should be set to blend probes if lighting is enabled.
