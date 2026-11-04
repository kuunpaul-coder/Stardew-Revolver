# Stardew Revolver

SMAPI code-mod prototype targeting Stardew Valley 1.6.15.3 and Android SMAPI 4.3.2.5.

## Current implementation

- Registers a custom weapon entry in `Data/Weapons`.
- Loads a custom transparent 16x16 pixel-art revolver sprite.
- Configures a travelling projectile with 40 base damage and an 8-tile range.
- GitHub Actions prepares Stardew Valley reference assemblies, generates the sprite, and compiles the project.

## Not finished or verified in-game yet

- Six-round magazine and reload logic.
- Dedicated firing animation and confirmed sound cue.
- Marlon's shop integration.
- Android touch-control verification.
- Final inventory and held-weapon appearance.

A successful CI build confirms compilation only. It does not prove that the weapon loads or behaves correctly in Android SMAPI.