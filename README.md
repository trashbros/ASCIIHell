# ASCIIHell
[![Build Status](https://api.travis-ci.com/trashbros/ASCIIHell.svg?branch=master)](https://travis-ci.com/trashbros/ASCIIHell)
[![unity version](https://img.shields.io/badge/unity%20version-2018.4.14f1-green.svg)]()

ASCII Art Bullet Hell

## Specifics
-All components of game are ASCII
-For phone/PC
-Single life, game over on bullet collision
-Scoring based on path and level completion
-Paths and bullet patterns generated at level initialization
-Player character customizer

## Controls
### Movement
- WASD
- Arrow keys
### Game Control
- E is start
- Q is pause/quit
### Attack
- F is fire
- X is slow time


## How-Tos
### Console Play
- Run the ASCIHellConsole visual studio project
- Resize window as necessary
- Play!
#### Local
#### Remote
### Viewer Play
- Run the ASCIIHellViewer visual studio project
- Resize window as necessary
- Play!

## Components
No health, your kill character gets touched and it's game over
### Path Generation
Generate random path with specific length and curvature difficulty
### Bullet Generation
Have resource, need to generate bullets at random patterns that leave the defined path open
### UI/Scoring
Score based on level completion?
### Character Generation
Key block for damage, and any extra characters for aesthetics

## Resources
[Bullet Generator](https://github.com/jongallant/Unity-Bullet-Hell)
