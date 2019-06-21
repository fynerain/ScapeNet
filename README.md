# ScapeNet
A custom solution built on Lidgren for video game networking in C#, geared towards Unity in the wake of UNET's demise.

ScapeNet is a small library, which allows the sending of organised sets of data in the form of 'packets', these make sending data more intuitive than the current system provided by Lidgren. The goal of ScapeNet is to make rapid prototyping of multiplayer games, fromn a lower level perspective, a more pleasant and intuitive process. ScapeNet is by default clientside orientated, but allows detailing on the server to make it more, if not fully serverside; however this must be done on a game to game basis.

# The Idea
I wanted a no nonsense, easy to use lower-level library for some small games i am developing in Unity. I felt using a string based identity system to register packets would be a fun and unique idea, allowing any type of packet to be setup and sent.

# Current Abilities

- Able to create custom packets, wich can hold any data Lidgren is able to send.
- Able to send and receive packets on either the client or server, and perfrom fully custom behaviours upon receving said packet.
- System to provide unique player id's and object id's, as well as keep those synced between all clients, as well as handle player joins and disconnects. (Unity_Networker)

# Limitations

There are a number of limitations, as well as complications in taking the approach i already have.

- Packet class definitions must be the same in both the client and server.
- Packets classes are defined in fairly similar and strict ways no matter the desired implementation.
- Some Lidgren knowledge is still required for certain things. It's especially needed for custom server and client implementations.
