
Rage Tanks
==========================

	Author Information:
		Written by _Ben Melanson & Josh Rand_
		For CS229 
		email: bmelanson@student.dwc.edu jrand@student.dwc.edu

  Introduction
  ------------
	This project is an implementation of the code given to us by the book to fix and improve upon.

### What We Implemented: 

- [10%] Currently the game plays in God Mode. Extend the game to have a variable number of lives configurable in Unity's Inspector with a default of 5. Visually display the lives remaining to the player. On the start screen make the game start in "Player" mode when you click on the Start button. God Mode should be supported by holding down the SHIFT key when clicking Start.
- [5%] Put in any background music that plays until the start button is pressed. Then transition to some other background music when the game is being played.
- [5%] Play a sound when bullets are fired.
- [10%] Add an event and event delegate object to the player. Add an event listener to the enemy. when the player is killed have the enemy play a sound (laugh, taunt, etc.).
- [5%] Add a particle system effect to the player when killed
- [5%] Do not allow enemies to collide with each other. Have them switch directions.
- [10%] Add at least 1 new platform with the ability to spawn enemies on it. Make sure the boss can also use the platform
- [5%] Have the player resurrect to any of the platforms.
- [5%] Play and audio clip when the player resurrects
- [5%] Only allow a vortex to spawn an enemy if there are less than 2 enemies on a platform.
- [5%] Have vortexes play a wind sound when spawning enemies.
- [5%] Have the boss play a "boss battle" sound when he spawns.
- [5%] Have the boss emit a particle effect when he lands on a platform.
- [5%] Have the boss slowly descend to the platform so that the player has a chance to move to a different platform. When the boss starts descending have him play a warning alarm.
- [5%] If the player touches the boss have him die (just like a normal enemy)

### What We Didn't Impliment:

- [10%] Instead of switching directions when the enemy gets to the edge of the platform, have it check the platform adjacent to it. If there enough room to jump to the adjacent platform without landing on another enemy then it should jump. If the platform has 2 enemies already patrolling it should not jump.
- [10%] Similar to the above, have the player play a (different) sound when an enemy is killed. (gotcha, bye bye, etc.). Play a yet again different sound when the boss is killed.
- [5%] Do not allow a vortex to spawn an enemy on the platform the player is currently standing on.
- [10%] Create a special gold enemy that spawns after 10 kills. If the player kills it credit 1000 points. 
- [10%] Add an event to the enemies such that when the boss dies all the enemies become angry and any platform that has less than 2 enemies spawns more enemies until every platform except the one the player is standing on spawns enemies.




