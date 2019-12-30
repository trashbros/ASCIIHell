# ASCIIHell
[![Build Status](https://api.travis-ci.com/trashbros/ASCIIHell.svg?branch=master)](https://travis-ci.com/trashbros/ASCIIHell)
[![unity version](https://img.shields.io/badge/unity%20version-2018.4.14f1-green.svg)]()

ASCII Art Bullet Hell

## Specifics
- All components of game are ASCII
- For phone/PC
- Single life, game over on bullet collision
- Scoring based on path and level completion
- Paths and bullet patterns generated at level initialization
- Player character customizer

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

## Implemented
- UDP console output
- console udp input
- image to ascii conversion
- player movement
- start and game over menus
- player lives
- time slow down charges
- enemies
- player fire and kill enemies
- score from firing

## To Implement
- Convert to server?
- Maybe define control keys on game (not level) start?
- Some sort of level generation
- high scores
- clean up slow downs and fine tune gameplay

## How-Tos
### Console Play
- Run the ASCIHellConsole visual studio project
- Resize window as necessary
- Play!

### Viewer Play
- Run the ASCIIHellViewer visual studio project
- Resize window as necessary
- Play!


## Resources
- [Bullet Generator](https://github.com/jongallant/Unity-Bullet-Hell)
- [Level Generation](https://arxiv.org/pdf/1806.04718.pdf)
- [Level Generation Github](https://github.com/amidos2006/Talakat)
