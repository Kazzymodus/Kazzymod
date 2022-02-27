using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Kazzymod.Buffs;
using Kazzymod.Projectiles.Minions;

namespace Kazzymod.Weapons
{
    class HydraStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Summon a Hydra to fight for you.");
        }

        public override void SetDefaults()
        {
            item.damage = 110;
            item.summon = true;
            item.mana = 50;
            item.width = 10;
            item.height = 10;
            item.useTime = 60;
            item.useAnimation = 60;
            item.useStyle = 1;
            item.noMelee = true;
            item.value = Item.buyPrice(0, 10, 0, 0);
            item.rare = 4;
            item.UseSound = SoundID.Item44;
            item.buffType = mod.BuffType<HydraBuff>();
            item.buffTime = 360;
        }

        public override bool UseItem(Player player)
        {
            KazzyPlayer modPlayer = player.GetModPlayer<KazzyPlayer>();
            var headId = -1;

            switch (modPlayer.hydraCount)
            {
                case 0:
                    Projectile.NewProjectile(Main.MouseWorld, Vector2.Zero, mod.ProjectileType<HydraBody>(), 1, 0f, player.whoAmI);
                    headId = Projectile.NewProjectile(Main.MouseWorld, Vector2.Zero, mod.ProjectileType<AcidHydra>(), 1, 7f, player.whoAmI);
                    break;
                case 1:
                    headId = Projectile.NewProjectile(Main.MouseWorld, Vector2.Zero, mod.ProjectileType<FireHydra>(), 1, 7f, player.whoAmI);
                    break;
                case 2:
                    headId = Projectile.NewProjectile(Main.MouseWorld, Vector2.Zero, mod.ProjectileType<MortarHydra>(), 1, 7f, player.whoAmI);
                    break;
                case 3:
                    headId = Projectile.NewProjectile(Main.MouseWorld, Vector2.Zero, mod.ProjectileType<GatlingHydra>(), 1, 7f, player.whoAmI);
                    break;
                default:
                    return true;
            }

            var head = (HydraHead)Main.projectile[headId].modProjectile;
            head.Initialise();
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
