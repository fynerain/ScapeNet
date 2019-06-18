# ScapeNet
A custom solution built on Lidgren for video game networking in C#, geared towards Unity in the wake of UNET's demise.

# The Idea
I wanted a no nonesense, easy to use high-level library for some small games i am developing in Unity. UDP was considered, but i didn't want to tackle the hassle that is resending, and dealing wth unordered packets. I felt using a string based identity system to register packets would be a fun and unique idea. Any type of packet can be setup and can be sent.

# Current Abilities of ScapeNet

-Able to send and receive packets on either the client or server.
-Able to customise behaviour apon receiving packet, on either the client or server.
-Able to create custom packets, wich can hold any data Lidgren is able to send.
-Able to provide unique player id's and object id's, as well as keep those synced between all clients. (Unity_Networker)

# Limitations

There are a number of limitations, as well as complications in taking the approach i already have.

-Packet class definitions must be the same in both the client and server.
-Packets classes are defined in fairly similar and strict ways no matter the desired implementation.
