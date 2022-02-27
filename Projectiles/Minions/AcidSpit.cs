using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Kazzymod.Buffs;

namespace Kazzymod.Projectiles.Minions
{
    public class AcidSpit : ModProjectile
    {
        private const int CorrosionDuration = 240;

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.minion = true;
            projectile.friendly = true;
            projectile.timeLeft = 120;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(mod.BuffType<Corroded>(), CorrosionDuration);
        }

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 4; i++)
            {
                int dustId = Dust.NewDust(projectile.position, projectile.width, projectile.height, 46);
            }
        }
    }
}
