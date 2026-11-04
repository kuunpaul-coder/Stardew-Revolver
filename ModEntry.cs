using System.Collections.Generic;
using HarmonyLib;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.GameData.Weapons;
using StardewValley.GameData.Shops;
using StardewValley.Tools;

namespace StardewRevolver;

public sealed class ModEntry : Mod
{
    private const string WeaponId = "StardewRevolver_Revolver";
    private const string TextureAsset = "Mods/kuunpaul-coder.StardewRevolver/Revolver";
    private const int MagazineSize = 6;
    private const int ReloadDurationTicks = 90;

    private static ModEntry? Instance;
    private readonly Dictionary<long, AmmoState> ammoByPlayer = new();

    private sealed class AmmoState
    {
        public int Ammo = MagazineSize;
        public int ReloadTicks;
    }

    public override void Entry(IModHelper helper)
    {
        Instance = this;
        helper.Events.Content.AssetRequested += OnAssetRequested;
        helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
        helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;

        var harmony = new Harmony(ModManifest.UniqueID);
        harmony.Patch(
            AccessTools.Method(typeof(MeleeWeapon), nameof(MeleeWeapon.leftClick)),
            prefix: new HarmonyMethod(typeof(ModEntry), nameof(BeforeWeaponClick)),
            postfix: new HarmonyMethod(typeof(ModEntry), nameof(AfterWeaponClick))
        );

        Monitor.Log("Stardew Revolver loaded. Six-round magazine and automatic reload enabled.", LogLevel.Info);
    }

    private static bool IsRevolver(MeleeWeapon weapon) =>
        weapon.QualifiedItemId == $"(W){WeaponId}";

    private static AmmoState GetAmmoState(Farmer who)
    {
        var mod = Instance!;
        if (!mod.ammoByPlayer.TryGetValue(who.UniqueMultiplayerID, out var state))
        {
            state = new AmmoState();
            mod.ammoByPlayer[who.UniqueMultiplayerID] = state;
        }
        return state;
    }

    private static bool BeforeWeaponClick(MeleeWeapon __instance, Farmer who, out bool __state)
    {
        __state = false;
        if (!IsRevolver(__instance) || !who.IsLocalPlayer)
            return true;

        var state = GetAmmoState(who);
        if (state.ReloadTicks > 0 || state.Ammo <= 0)
        {
            if (state.ReloadTicks <= 0)
                state.ReloadTicks = ReloadDurationTicks;
            return false;
        }

        __state = !who.UsingTool;
        return true;
    }

    private static void AfterWeaponClick(MeleeWeapon __instance, Farmer who, bool __state)
    {
        if (!__state || !IsRevolver(__instance) || !who.IsLocalPlayer)
            return;

        var state = GetAmmoState(who);
        state.Ammo = System.Math.Max(0, state.Ammo - 1);
        if (state.Ammo == 0)
        {
            state.ReloadTicks = ReloadDurationTicks;
            Game1.playSound("objectiveComplete");
            Instance?.Monitor.Log("Revolver empty. Automatic reload started.", LogLevel.Debug);
        }
    }

    private void OnUpdateTicked(object? sender, UpdateTickedEventArgs e)
    {
        foreach (var state in ammoByPlayer.Values)
        {
            if (state.ReloadTicks <= 0)
                continue;

            state.ReloadTicks--;
            if (state.ReloadTicks == 0)
            {
                state.Ammo = MagazineSize;
                Game1.playSound("objectiveComplete");
                Monitor.Log("Revolver reloaded: 6/6.", LogLevel.Debug);
            }
        }
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
                    Projectiles = new List<WeaponProjectile>
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
        ammoByPlayer.Clear();
        Monitor.Log("Revolver data and six-round magazine are ready.", LogLevel.Info);
    }
}
