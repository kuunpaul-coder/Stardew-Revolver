using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.GameData.Weapons;
using StardewValley.GameData.Shops;

namespace StardewRevolver;

public sealed class ModEntry : Mod
{
    private const string WeaponId = "StardewRevolver_Revolver";
    private const string TextureAsset = "Mods/kuunpaul-coder.StardewRevolver/Revolver";

    public override void Entry(IModHelper helper)
    {
        helper.Events.Content.AssetRequested += OnAssetRequested;
        helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;

        Monitor.Log("Stardew Revolver loaded. Custom weapon data registration is enabled.", LogLevel.Info);
    }

    private void OnAssetRequested(object? sender, AssetRequestedEventArgs e)
    {
        if (e.NameWithoutLocale.IsEquivalentTo(TextureAsset))
        {
            e.LoadFromModFile<Texture2D>("assets/revolver.png", AssetLoadPriority.Exclusive);
            return;
        }

        if (e.NameWithoutLocale.IsEquivalentTo("Data/Shops"))
        {
            e.Edit(asset =>
            {
                var shops = asset.AsDictionary<string, ShopData>().Data;
                if (!shops.TryGetValue("AdventureShop", out var guildShop))
                    return;

                if (!guildShop.Items.Any(item => item.Id == WeaponId))
                {
                    guildShop.Items.Add(new ShopItemData
                    {
                        Id = WeaponId,
                        ItemId = $"(W){WeaponId}",
                        Price = 15000,
                        AvailableStock = -1
                    });
                }
            });
            return;
        }

        if (e.NameWithoutLocale.IsEquivalentTo("Data/Weapons"))
        {
            e.Edit(asset =>
            {
                var weapons = asset.AsDictionary<string, WeaponData>().Data;
                weapons[WeaponId] = new WeaponData
                {
                    Name = "Stardew Revolver",
                    DisplayName = "Six-Shooter",
                    Description = "A sturdy six-shot revolver. Fires a fast projectile.",
                    Type = 0,
                    Texture = TextureAsset,
                    SpriteIndex = 0,
                    MinDamage = 35,
                    MaxDamage = 50,
                    Knockback = 1.1f,
                    Speed = 0,
                    CritChance = 0.02f,
                    CritMultiplier = 3f,
                    CanBeLostOnDeath = false,
                    MineBaseLevel = -1,
                    MineMinLevel = -1,
                    Projectiles =
                    new List<WeaponProjectile>
                    {
                        new WeaponProjectile
                        {
                            Id = "revolver_bullet",
                            Damage = 40,
                            MaxDistance = 8,
                            Velocity = 16,
                            RotationVelocity = 0,
                            TailLength = 0,
                            FireSound = "gun",
                            CollisionSound = "clubhit",
                            SpriteIndex = 11
                        }
                    }
                };
            });
        }
    }

    private void OnSaveLoaded(object? sender, StardewModdingAPI.Events.SaveLoadedEventArgs e)
    {
        Monitor.Log("Revolver data is ready. Shop and six-round reload mechanics are still pending.", LogLevel.Info);
    }
}