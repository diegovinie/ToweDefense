

## Ep 09
- When the missile doesn't match the coord system, create a GO as parent, check the scale, rotate the missile until it matches the parent.

- `Physics.OverlapSphere()` returns and a array with all GO touching the sphere.

- Use gizmos for the range `OnDrawGizmosSelected()` and `Gizmos.DrawWireSphere()`

- With particle system:
    - Set loop on while you are tweaking it.
    - Drag material to the particle system
    - You can set a child (flame), but work on each separately
    - Interesting:
        - General
            - Start Size m/M
            - Start Speed m/m
            - gravity Modifier
            - Start Life m/M

        - Shape
            - Radius
        - Emission
            - Bursts Min
        - Renderer
            - Disable cast shadows and receive shadows
- Light
    - Add Point Light
        - Adjust color
        - Intensity
    - Add Animation window
    - Add new Animation: it creates animation and controller
    - Remember press record in animation panel
    - Change frames then the properties
