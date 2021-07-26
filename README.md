# l-system



## about

![procedurally generated tree](https://raw.githubusercontent.com/eastriverlee/l-system/master/screencapture.gif)

this is a proof of concept l-system tree/plant generator using lineRenderer of unity engine.  
[video demonstration](https://youtu.be/Fqe_OUSSPmI)  

## how to run (in unity)

1. copy scripts to your projects.
2. add a `+ > Empty` gameObject of any name with `Lsystem.cs` script attached.
3. make a prefab of  `+ > Effects > line` that has 1 `positions > size`, making it invisible.  
4. adjust width and end cap vertices *(ex: 0.02, 10)* of the prefab.
5. attach `Twig.cs` script to the prefab.
6. set `Branch` parameter of the first added gameObject as the prefab.
7. BUILD && RUN.

