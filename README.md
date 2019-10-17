# ScapeNet
A small library built on Lidgren for sending larger amounts of data at a time with ease. With Unity support for video game networking. Geared for a more low level approach.

ScapeNet is a small library, which allows the sending of organised sets of data in the form of 'packets', these make sending data more intuitive than the current system provided by Lidgren. The goal of ScapeNet, is to make rapid prototyping of multiplayer games, from a lower level perspective, a more pleasant and intuitive process. ScapeNet is by default clientside orientated but allows detailing on the server to make it more, if not fully serverside; however this must be done on a game to game basis.

# The Idea
I wanted a no non-sense, easy to use lower-level library for some small games i am developing in Unity. I felt using a string based identity system to register packets would be a fun and unique idea, allowing any type of packet to be setup and sent.

# Current Abilities

- Able to create custom packets, which can hold any data Lidgren is able to send.
- Able to send and receive packets on either the client or server, and hook fully custom behaviours upon receving said packet.
- System to provide unique player id's and object id's, as well as keep those synced between all clients, as well as handle player joins and disconnects. (Unity_Networker)

# Limitations

There are a number of limitations, as well as complications in taking the approach i already have.

- Packet class definitions must be redefined in both the client and server if the codebases are seperate.
- Packets classes are defined in fairly similar and strict ways no matter the desired implementation.
- Some Lidgren knowledge is still required for certain things. It's especially needed for custom server and client implementations.

# Unity Example

To run the Unity example do the following:
 - Download and open the example Unity project given in the source.
 - Open the SampleScene.
 - Click on ScapeNet on the top bar and set it to Client + Server (this gives you both the client and server in a build).
 - Run the game.
 
To open just a Client/Server build press the Client or Server button. If its a server build, be sure to press the server build checkmark in the build settings.
