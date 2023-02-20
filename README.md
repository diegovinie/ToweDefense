

# Notes

## Ep 10
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


## Ep 11
- `[System.Serializable]` allows Unity to parse the structure and displays it on the parameter config
- `[Header("Optional")]` lets the user to know that it's and optional parameter
- Property `CanBuild` acts like a computed prop
- `Quaternion.identity`
- In `PlayerStats` static properties can be modified (Money)
- Nodes can hold a turret from the beginning, setting it in parameters


## Ep 12
- Import fonts: in font inspector properties: Create TMP Font asset, generate and save
- Canvas -> canvas -> Render Mode: World Space
- Text -> stretch (hold alt and click)
- `Math.Clamp()` and `string.Format()`
- For Text Mesh Pro:
    - `using UnityEngine.UI;`
    - `using TMPro;`
    - `TextMeshProUGUI` type
- Build Effect:
    - Reduce size with time
    - Shape hemisphere
    - Check burst

## Ep13
- EnemyDeathEffect - take into account:
    - renderer: drop material
    - size over time
    - emission: rate over time = 1

## Ep14
- Import **Laser Beamer**:
    - Import new asset in the created folder
    - scale = 0.5 and apply
    - In materials:
        - location: Use External Materials (Legacy) and apply, they will show up in the Materials folder
- When importing the texture:
    - Texture Type: Sprite 2D and UI
    - Sprite Mode: single nad apply
- Line Renderer Component added to the LaserBeamer
    - Drop a Material
    - change the positions

## Ep15
- Laser Impact
    - Use shape = cone to
    - material: activate Emission, global ilumination: off
    - For better performance, In collision:
        - create a new layer called environment, assign it to nodes and ground
        - collide with: environment (only)
    - Set the direction towards the firePoint

- You can nest particle systems

- LaserGlow
    - Color over lifetime: fade out
    - material: render = additive (legacy)
        - texture: default-particle

## Ep16
- Let's move the movement mechanism to its own script
- Use `[RequireComponent(typeof(Enemy))]` when you want a strict dependency
- `[HideInInspector]`
- To ensure that EnemyMovement will be run after Enemy (a common practice for debuff)
    - Go to Project Settings -> Script Execution order
    - Add EnemyMovement 100ms after


## Ep17
- Game Object (stretched) -> panel -> text:
    - Create TMP font
- Shadow in text not working
- OverlayCanvas -> Canvas scaler:
    - UI Scale Mode: Scale with screen size
    - Screen Match mode and move all to **height** (disabled because turrets look stretched)
- **Static variables** carry on from one scene to another
- Use a temporary end game key
- Scene Management:
    - Go to File > Build Settings
    - Drop the MainScene and notice the index
- Tune the highlight of the buttons
- Animation:
    - Background:
        - Add Canvas Group Component (change the alpha without messing around with the color)
    - Game Over text: come down and fade in
        - Add Canvas group
    - Round survived: scale and fade in
        - Add Canvas Group
    - Buttons: pop up
        - Add Canvas Group
        - tweak size and alpha
- Problem with standard turret, no line renderer assigned.

## Ep18
- Canvas
    - In the nodeUI, GameObject must be placed where the turret is and the canvas can be repositioned
    - Render Mode: World Space
    - In Text boxes we can use html tags
    - In the inspector, selecting Debug we can see the private variables

## Ep 19
    - In any canvas, Graphic Raycaster overlaps with the pointing events.

## Ep 20
- I messed up with the sellEffect, and I had to undo changes in the file from git history.

## Ep 21
- The best way to pause is `Time.timeScale = 0f;`
- Animator:
    - Update Mode: Unscaled time (to prevent freezing when delta time is 0)
    - For buttons we can create an animator controller (with all states)

## Ep 22
- To place the camera in the view: Camera selected and `Crtl + Shift + F `
- Break prefab instance in modern Unity: `Prefab > Unpack completely`
- To assign the existing button animation: Add Animator component and for controller select Button
- Add the new scene to Build settings (first slot)

## Ep 23
- In an image, when we use a sprite we can control the fill with fillAmount, we need a tiny white sprite.
- White sprites can work with any color (color * 1 = color)
