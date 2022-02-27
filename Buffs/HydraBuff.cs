using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Kazzymod.Projectiles;
using Kazzymod.Projectiles.Minions;

namespace Kazzymod.Buffs
{
    class HydraBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Hydra");
            Description.SetDefault("\"All that blood... they're gonna swallow you whole!\"");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.maxMinions += 10;

            KazzyPlayer modPlayer = player.GetModPlayer<KazzyPlayer>();

            if (player.ownedProjectileCounts[mod.ProjectileType<HydraBody>()] > 0)
            {
                var hydraCount = 0;

                for (var i = 0; i < 1000; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].modProjectile is HydraHead)
                    {
                        ++hydraCount;
                    }
                }

                modPlayer.hydraCount = hydraCount;

                modPlayer.hydraMinion = true;
                player.buffTime[buffIndex] = 18000;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}
