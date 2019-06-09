# ScapeNet
A custom solution built on Lidgren for video game networking in C#, geared towards Unity in the wake of UNET's demise.

# The Idea
I wanted a no nonesense, easy to use TCP library for some small games i am developing in Unity. UDP was considered, but i didn't want to tackle the hassle that is resending, and dealing wth unordered packets. I felt using a string based identity system to register packets would be a fun and unique idea. Any type of packet can be setup and can be sent.
