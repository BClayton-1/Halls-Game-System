# Halls Game System
## Prototype for a game made in the Unity Engine.
Involves visual novel style 2D sections with first-person 3D sections and easy transition between the two.

Includes:
- Dialogue display with ability to change backgrounds, background music, play sounds, transition between scenes
- A day and time system
- An inventory system that allows for the creation of new 'possessions' through an XML file

## Dialogue Text Files

Dialogue is parsed from text files rather than being hard-coded. These dialogue files are designed to be possible to write for people unfamilliar with coding.

Rather than being sequential, dialogues are chosen based on an event system. Dialogues may have conditions which must be met for them be added to the pool.

Each dialogue has a header, which contains information used by the event system, and a body, which contains the actual dialogue to be displayed as well as various commands

#### Example Dialogue File:
![dialoguetxt](https://github.com/user-attachments/assets/4bef93cb-b7d0-4f67-9887-f40f1a66b9b3)

#### Result:
![dialogueresult](https://github.com/user-attachments/assets/7879400e-9b5a-456e-98e9-5be861b10c78)


There is also support for branching dialogue using the following format:

![branchtxt](https://github.com/user-attachments/assets/c288f2cd-0ddc-40c8-9ef6-d63dd68dab49)

ENTER3DROOM() is used to transition to a 3D scene

![enter3droom](https://github.com/user-attachments/assets/81c4b38b-1af9-47ef-ba6b-610c71c220ab)

The doors seen in the above image have interact triggers on them that can either switch to another 3D scene or enter a 2D scene on use.

## Possessions
Possessions are things the player can have in their inventory. They can be added or removed using dialogue commands. They can also be used by the condition checker.

Possessions may also have one or more 'Usable components', which define right-click functionality. The only currently implemented Usable component is the Instant component, which may add or remove possessions on use.

#### Example of a possession with Usable components being right clicked:
![rightclick](https://github.com/user-attachments/assets/75435481-1670-443d-95f4-d48ea3210564)

Possessions are defined in possessions.xml
#### Two possessions in possessions.xml
![possessionxmlex](https://github.com/user-attachments/assets/1cde7710-b708-4565-a668-3156c9acb3d0)

Seen here is the xml for empty_jar, and jar_of_honey, which has two Instant components that when clicked, remove 1 jar_of_honey and add 1 empty_jar to the player's inventory.

