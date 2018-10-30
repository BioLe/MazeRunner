"# MazeRunner" 

Unity simulation where bots with a simple DNA try to learn to go as far as they can in a maze during 't' seconds.

DNA has two genes [distanceToTravel, rotation].

Every gen the agents are ordered by distance travelled, and the first 25 are bred with each other. Forming a new wave of 50 agents with the best characteristics of the old ones.

Example of how breeding works:

Bot1[149,128]
Bot2[265,154]
Bot3[232,141]

New bots based on bot1 and bot2:
Bot3[149,154]
Bot4[265,128]

New bots based on bot2 and bot3:
Bot3[265,141]
Bot4[232,154]

