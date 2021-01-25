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

## Step4

## Before:
The UI creation is more complicated than I expected in 3D. I managed to put some janky panes on the screen, but those need to be changed.
 Again, :

* Make a stable UI , with a label and a button. When the button is pressed:
* Create a bone.

## After:

Learned a bit about UI. Still won't scale as I expect, but it's functional enough. I have text and a button.
Also, I managed to create objects dinamically, and to assign parents to said objects. Unity is great. You just draw the script on an object and it adds it as a component.
For now I have a plane of symmetry and two child objects.

## Step4

## Before
I finally can start a little bit of the generation stuff. I'll try to have some procedural generation of legs for the next step.
After this I will work to make multiple symetry easier to implement

## After
I did a little procedural generation. Currently it is really constrained, so you get something between a multi-legged minotaur and a human. This constraints can be easily tweaked, but I added them to have more concrete results. Code cleanup should be the next stage, and after serialize all the variables (also the direction pointer should be removed from the skeleton)
I'm pretty satisfied with the results. The procedural generation part should go really smooth from now on. The animation and the AI part are the next obstacles. (Also interfaces will be hard to manage)

## Step5

## Before
Clean up the code. make more parameters random (Add multiple legs per spine segment), remove the direction pointer.

## After

* I did the code cleanup.
* I implemented multiple spineSegments for arms(from now on called verticalJoints).
* I implemented the head.

## Step6>

## Before

What's nice about the current state of the project is that just by generating a few creatures you can clearly see what you could add next,
and if it's interesting enough to be added. More than that the code is modular enough to facilitate changes.
The next updates will be small features added, until I think the generator has the majority of the components I wanted to include.

