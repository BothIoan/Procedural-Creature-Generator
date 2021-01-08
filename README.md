# Niggurath

##Description

This is my uni final project. I want to make a creature random generator using
the unity engine. This Readme will be (for now) a progress log.

##Ste1

Got unity, managed to create a terrain 
-> Will make the walk inverse kinematic look better
-> Later It would be really nice if I gave the user some backgrounds to choose from

##Step 2

###Before:
I gotta learn how to create a rig in unity, and add it to the project. Unfortunately, this thing is usuallt made in blender. This video might be the solution:
[spore creature builder](https://www.youtube.com/watch?v=Br_SQAc87s8).

###After:
-> I linked visual studio to unity, so that scripts can detect unity objects.
-> I added bones to a cube
-> I found a way to draw a skeleton in unity. For some reason there is no good way to draw simple lines, and the only one that seemed to work couldn't be used in builds (if you don't pay for it). At last, I found some guy who reimplemented the Ghizmo.renderLine method, and I wrote a script to draw lines between components of a skeleton and their parent component.

##Step 3>

##Before:
There are 3 things that I know I don't know how to do in unity : draw lines, make user interfaces, create new bones in-game. Now there are only two things I know I don't know. Therefore:

-> Make a simple user interface
-> Create a bone in-game
-> Clean up the code in the DrawSkeleton script
-> make camera rotate around the skeleton
