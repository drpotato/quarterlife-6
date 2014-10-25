============
 Flying Fit
============

Team HELI Members:
==================

 * Chris Morgan    - https://github.com/drpotato   - Game Feels Engineer
 * Timothy Diack   - https://github.com/boots33    - Project Lead and Creative Director
 * Chris Pearce    - https://github.com/cjpearce   - Master Terraformer
 * Jonathan Curtin - https://github.com/denseNinja - Supreme Supervisor of Stuff

===================
 Building the game
===================

Please ensure that when you build, the three scenes are all included in the order:
Menu - Scene 0
Regular - Scene 1
Cognitive - Scene 2

Alternatively if things are not working very well, just build the regular and 
cognitive scenes by themselves. The memory game ONLY works if Oculus Rift is attached
and functioning.

==========
 Controls
==========

The controls are sometimes quite finicky (mostly in the Menu scene, press/hold buttons
if they do not immidiately work.). If the exercycle is attached, ensure that the COM 
port is the same as the BikeController class.

 Action:	| Keyboard:	| Joystick:	
================|===============|===============
 Increase Lift 	| W		| Throttle
 Decrease Lift 	| S		| Throttle
 Turn Left 	| A		| Z-Axis
 Turn Right 	| D		| Z-Axis
 Reset 		| R		| Button 3
 Forward Pitch 	| Up		| X-Axis
 Backward Pitch | Down		| X-Axis
 Tilt Right 	| Left		| Y-Axis
 Tilt Left 	| Right		| Y-Axis
 Exit		| Escape	| Button 7
 Select Option	| Enter		| Trigger

===============
 Contributions
===============

For the record, we would like to state that the number and size of commits did not
always reflect who did what in our project. Almost all of these contributions were 
collaborative to some degree. Also some contributions did not make the final prototype.

Basically we think that we all did approximately equal amounts of work.

Timothy Diack:
==============

- Final helicopter model
- 3 courses (including final terrain layout)
- 4 custom textures
- 1 block asset
- Audio
- Game logic scripting

Jonathan Curtin:
================

- Rings + final ring layout
- Ring triggers
- Sorted out hardware
- Menu: Layout and scripting/control scheme

Christopher Pearce:
===================

- Initial helicopter model
- Flight physics/controls/scripts
- Final terrain + textures

Christopher Morgan:
===================

- Cycle integration 
- Reset function (for when the helicopter flips over)
- Helicopter controls
- Some scenes we scrapped.



