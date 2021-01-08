# Niggurath

## Description

This is my uni final project. I want to make a creature random generator using
the unity engine. This Readme will be (for now) a progress log.

## Step1

Got unity, managed to create a terrain 
* Will make the walk inverse kinematic look better
* Later It would be really nice if I gave the user some backgrounds to choose from

## Step 2

### Before:
I gotta learn how to create a rig in unity, and add it to the project. Unfortunately, this thing is usually made in blender. This video might be the solution:
[spore creature builder](https://www.youtube.com/watch?v=Br_SQAc87s8).

### After:
* I linked visual studio to unity, so that scripts can detect unity objects.
* I added bones to a cube
* I found a way to draw a skeleton in unity. For some reason there is no good way to draw simple lines, and the only one that seemed to work couldn't be used in builds (if you don't pay for it). At last, I found some guy who reimplemented the Ghizmo.renderLine method, and I wrote a script to draw lines between components of a skeleton and their parent component.

## Step 3

## Before:
There are 3 things that I know I don't know how to do in unity : draw lines, make user interfaces, create new bones in-game. Now there are only two things I know I don't know. Therefore:

* Make a simple user interface
* Create a bone in-game
* Clean up the code in the DrawSkeleton script
* make camera rotate around the skeleton

## After:

Wrote a rotate object script. I might want to also rotate the ground, and to have rotation speeds which slowly get to 0 each frame when the button is not pressed.
Cleaned the DrawSkeleton script. Later I might change the skeleton drawing, to make it look smoother.
Made color of skeleton and rotation speed serializable. I can let the user change them later.

## Step4>

## Before:
The UI creation is more complicated than I expected in 3D. I managed to put some janky panes on the screen, but those need to be changed.
 Again, :

* Make a stable UI , with a label and a button. When the button is pressed:
* Create a bone.

