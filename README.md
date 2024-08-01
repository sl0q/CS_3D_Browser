# CS_3D_Browser
This is my course project for Computer Graphics discipline.
Browser for 3D objects in C# using only Windows Forms. Can only work with .obj files.

# CONTROLS
W - move forward, away from POV  
S - move backward, to POV  
A - move left  
D - move right  
C - move down  
Space - move up  
R - scale up  
F - scale down  
Arrows + QE - rotation around local origin  
IJKL + UO - rotation around global origin  

# /
You can select a file using a button in top left corner. There are some samples in \KG_LR_2.2\Samples.
The object can be rendered in orthoganal or perspective projections
You can chose the way of handling invisible lines: Default (wireframe), Roberts' algorithm, Z-Buffer.

Roberts' algorithm works properly only on objects with only convex edges.
Z-buffer can be laggy if it has to paint over a big chunk of screen.

For matrix operations I used my own marix library. Implemented some basic operations there, so I could use general */-+ operators with 2D arrays.
