## Visitor Pattern Implementation

Overview

The Visitor Pattern has been employed to handle interactions between the player and collectible items within the game, such as `CureCollectible`. The pattern facilitates the addition of new types of collectible items without the need to modify the player's code, adhering to the Open/Closed Principle.

Classes and Interfaces

- `ICollectible`: Interface for all collectible items.
- `IPlayerVisitor`: Interface for the player to implement visitor functionality.
- `CureCollectible`: A concrete class for a collectible item that represents a cure.

## The design of prototype

Warriors in the game engage in combat with Titans, using weapons found within the environment. The game features harmless animals that roam the scene but can be commanded by the Titan player to join the fight.

**Lobby and Networking**

The game includes a lobby system for players to join and create game sessions. The networking supports cross-platform play between PC and VR users.

**Gameplay**
Warriors must locate and use weapons from the ground to defeat the Titans.

**Titan Controls**
Titans have the ability to command animals in the game, transforming them into threats against the Warriors and grab trees to attack players.

**VR and PC Interaction**
Players on VR have immersive controls, while PC players use traditional keyboard and mouse or gamepad controls. The game ensures compatibility and fair play between these different control schemes.

**Environment**
Animals move freely across the game scene, adding to the game's ambiance and strategy when they are turned into adversaries by the Titan.
