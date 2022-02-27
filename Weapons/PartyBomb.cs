using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using Kazzymod.Projectiles;

namespace Kazzymod.Weapons
{
    public class PartyBomb : ModItem
    {
        private const float throwVelocityX = 2f;
        private const float throwVelocityY = 16f;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("\"Let's party like it's 2019!\"");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.useTime = 60;
            item.useAnimation = 10;
            item.useStyle = 1;
            item.noMelee = true;
            item.value = Item.buyPrice(0, 10, 0, 0);
            item.rare = 8;
            item.UseSound = SoundID.Item1;
        }

        public override bool UseItem(Player player)
        {
            KazzyPlayer modPlayer = player.GetModPlayer<KazzyPlayer>();

            modPlayer.partyBombActive = false;

            if (Filters.Scene["Confetti"].Active)
            {
                Filters.Scene["Confetti"].Deactivate();
            }

            //Vector2 direction = Vector2.Normalize(Main.MouseWorld - player.position);

            Projectile.NewProjectile(player.position, new Vector2(throwVelocityX * player.direction, -throwVelocityY), mod.ProjectileType<PartyBombProjectile>(), 0, 0, player.whoAmI);
            return true;
        }

        public override bool AltFunctionUse(Player player)
        {
            KazzyPlayer modPlayer = player.GetModPlayer<KazzyPlayer>();

            modPlayer.partyBombActive = false;

            if (Filters.Scene["Confetti"].Active)
            {
                Filters.Scene["Confetti"].Deactivate();
            }

            return false;
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
