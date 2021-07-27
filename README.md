# l-system



## about

![procedurally generated tree](https://raw.githubusercontent.com/eastriverlee/l-system/master/screencapture.gif)

this is a proof of concept of l-system tree/plant generator or a growth simulator using lineRenderer of unity engine.  
[video demonstration](https://youtu.be/Fqe_OUSSPmI)  

## how to run (in unity)

1. copy scripts to your projects.
2. add a `+ > Empty` gameObject of any name with `Lsystem.cs` script attached.
3. make a prefab of  `+ > Effects > line`.
4. adjust end cap vertices of the prefab if you wish.
5. attach `Twig.cs` script to the prefab.
6. set `Branch` parameter of the first added gameObject as the prefab.
7. **BUILD && RUN**.
