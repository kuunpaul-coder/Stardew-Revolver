# Stardew Revolver

An Android-targeted SMAPI mod prototype for Stardew Valley 1.6.15.3 / SMAPI 4.3.2.5.

## Current status

This is only the initial loader diagnostic prototype:
- SMAPI manifest and C# project entry point
- Startup log message to confirm that SMAPI loads the DLL

**Not implemented yet:** revolver item, inventory icon, Marlon shop integration, projectile shooting, damage, six-round ammunition, reload behavior, firing animation, and sound.

## Build and testing

Build with the .NET SDK and the Stardew Valley ModBuildConfig package. The resulting mod must be tested on the target Android SMAPI installation before compatibility can be claimed.

## Planned gameplay

- Six-shot revolver
- Ranged projectile attack (not melee and not a reskinned slingshot)
- Target damage around 35–50
- Reload after the cylinder is empty
- Pixel-art inventory icon and firing effects
- Obtainable through Marlon's shop
