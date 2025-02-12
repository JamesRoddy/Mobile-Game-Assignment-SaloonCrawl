This project is a group project that was developed for mobile as part of a univeristy module of which the group achived a first for the project 
This project was made in unity using c#

The game: 
Saloon crawl is a 2d auto runner with a western style theme where the player is tasked with making it as far as possible running through deserts, saloons, shooting zombies, kicking and jumping over various obstcales like cacti or bar stools, and also sliding under flying beer bottles. 
All while increasing their score as they go.

main features: 
scaling UI with differeing screen size 
terrain generation with object pooling 
camera functionality through touch 


My responsbilites for impelmentation:

Terrain generation algorithm with object pooling: 
As we were making a 2D auto runner it was decided ealry on that the game would require some kind of procedural level generation as the player progressed and this was my main task for this patricualr project, the system 
was desigined wih various properties including the fact that all objects(enemies,obstacles,ground,etc) used during the procedural generation were object pooled meaning they were all instianted at the begining of the game and then repositioned and 
deactivated as necsessary as the player prorgressed further ensuring that there was very little performance impact as the player moved from terrain to terrain as all the objects were already instanited in memory and 
were not constanly being created and destoyed. The terrian generation also features adjacency rules, randomness, and spawn condtions for specifc terrain  keeping things varied. 

Event system: 
I also desgined and implmented and event system based on timers that would generate random obstacles for the player as they were running through each terrain 
this system also ensured that all objects associated with each event were object pooled maintaining consistency with the terrain genertaion algorithm and reducing the impact that generating an event had 
while the player was prorgessing through out the various biomes they encountered.
