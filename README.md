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

## Step6

## Before

What's nice about the current state of the project is that just by generating a few creatures you can clearly see what you could add next,
and if it's interesting enough to be added. More than that the code is modular enough to facilitate changes.
The next updates will be small features added, until I think the generator has the majority of the components I wanted to include.
To Add:

* Complete leg bone structure
* Complete arm bone structure
* Add multiple arms per vJoint
* Add multiple legs per vJoint
* Add different angles to head
* Add spider legs
* Add different angles to spines

## After

Added all of the features above more or less + another small feature just for my fun which assigns funny names to each creature (depending on your sense of humor.) I love how they look now. Next I'll try to polish the just implemented features.

## Step7

## Before

* Make hSpines incline-able.
* Make spider legs more modular, so that there can be 1-3 legs per joint

# After
I'm very happy with the results. I made the hSpines incline-able.

## Step8

## Before

Spidery creatures are generated too often. I might will the chance of one spawning
A lot of constraints need to be enforced so that members will overlap. I will dedicate a step to this in the future. For now
I will make the legs and hands impossible to overlap. The names feature is really funny. Showed it to friends and got more ridiculous words to put in the app.

## After

For placing constraints, so that members can't overlap I needed a better sense of scale. And for that I needed to see how the creatures would look when textures are placed on them. After paining myself with the mathematics, I finally was able to put bone textures between the joints. I also placed some skulls on the heads of the creatures. It was really worth it. The creatures look really good now, and there's enught diversity between them that you could think this is the final product

## Step 9

## Before
Skipped writing

## After
* I added wings and tails. Wings took a lot of time, because they have complex shapes that are hard to describe at runtime. To describe their curves you need combinations of exponential and liniar functions. I started on a wrong path at first, describing them manually, just to get a sense of what a wing is. Fortunatelly I managed to generate some wings that look above decent, and can be improved. These are the bird-type wings.(some bones and bones representing feathers attached to them).
* I also added bat-like wings (hands in the form of wings). Those are too much of a fixed structure. If you would try to describe them through functions it would take a lot of time and you would get little to no space for valuable variations. At a later time I will make their general size and length variable.
* I added the first animation (a head rotation) to the project. This took a lot of time, because animations are usually added in blender, when creating the character. In unity you are provided with rigging animations which help a lot, because they can be generated in unity. Unfortunately, their main use is in editor mode (with drag and drop tools). The Scripting API is very poorly documented, and I found no tutorials on the subject of scripting this animations, so I had to figure out by myself a way to do this.
* Animations get complicated with instantiated objects because their rotations are not applied, and you can't apply them in unity. The way I generate creatures is through first placing the joints and then, between the joints, instantiating the bones. This helps me at animations because the joints are not rotated, but it might have an negative impact on the performance/larger objects when exporting, so I might want to find a solution for this later. Through trying to fix this, I accidentally Found a way to export prefabs(type of object readable only in unity), and how to convert those prefabs into fbx files(readable in blender).
* In short : I added wings, tails, learned how to animate dynamically and added an animation, found a way to export prefabs and fbx files containing the creatures. Also added spikes as an alternative to bones for some bodyparts

## Step 10 

## Before
Now is the time to add all the animations. More than half of the researching is done from step 9, but I still have to learn how to do complex IK on my weird representation of the creatures.

## UPDATE
Took a long break on this. First and foremost I want to reorganize my code, to make it more efficient/readable.

## After
* Instead of having one big file, I now have 10+ files, with much lower coupling. <br>
There are now separate stages for:<br>
 >TopLevel (The big generator/grammar)<br>
 >Modules (Smaller generators)<br>
 >Animations<br>
 >Exporting<br>
 >Setup (Initial operations)
 * A walking animation is fully added.
 * Fixed some bugs with the grabbing animation.
 * Wrote a serializer class for the machine learning.

## Step 11

## Before >
* Finally, the last complex functionality of the project, the machine learning. This can be approached in two ways.
>I either do a big model containing all the possibilities, while zero-padding the others.<br> 
>Or I do many smaller models, for each module.<br>
For simplicity's sake, I started with the first approach, but now I think many smaller models are not only the more flexible and updatable solution, but also more easy to manage/ implement, as I won't have to think about the zero padding.<br>

 * So My next move is to be able to serialize each module + the grammar. (Modules are quite similar, to the point where I think having them as a classes rather than methods, all inheriting some super class, would help a lot in the future.)
 * Also I need to break the legs module into two separate modules (spidery/ not spidery, because the current way of doing this is not consistent)

## After 
* Ended up transforming all modules methods into classes, inheriting an interface. Now it works as before, with almost no overhead, and with the benefit that I can now add all modules in a list, together with it's structures, which will help for a truly flexible grammar.
* The modules list also helps with having multiple ML constraint solvers, as I can do the required operations on lists as oposed to individual objects.
* **At some point I must fix the legs generation, as it generates too weird legs almost half of the time**

## Step 12

## Before
* I have calsses for each module. I should:
  > Add a class serialization method.
  > Implement some algorithm for auto-generation of the model, and add it to the generation method of each module

## After 
* I implemented and tested serialization.

## Step 13

## Before >
* Maybe the values must be normalized, maybe they should be written in CSV, but besides that, the Unity side for input is over.
* Now, I must first do my research, and then write a python script for one constraint solver.
 