using System;
using Kazzymod.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Kazzymod.Weapons
{
    class Yurnero : ModItem
    {
        public float DashSpeed = 60f;

        public override void SetDefaults()
        {
            item.damage = 100;
            item.width = 28;
            item.height = 30;
            item.useTime = 240;
            item.useAnimation = 20;
            item.crit = 200;
            item.useStyle = 3;
            item.melee = true;
            item.noMelee = true;
            item.UseSound = SoundID.Item1;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Yurnero");
        }

        public override bool UseItem(Player player)
        {
            player.velocity.X += DashSpeed * player.direction;
            player.GetModPlayer<KazzyPlayer>(mod).yurneroDash = true;
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.DirtBlock);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
