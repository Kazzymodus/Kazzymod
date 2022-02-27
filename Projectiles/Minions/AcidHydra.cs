using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Kazzymod.Projectiles.Minions
{
    class AcidHydra : HydraHead
    {
        private const float SpitSpeed = 10;
        private const int SpitDamage = 10;
        private const int SpitKnockback = 3;

        protected override TargetMode TargetPriority => TargetMode.ClosestToHydra;

        protected override void Attack()
        {
            var target = Main.npc[Target];

            var direction = target.Center - projectile.position;
            var velocity = Main.projectile[BodyId].velocity + Vector2.Normalize(direction) * SpitSpeed;

            Projectile.NewProjectile(projectile.position, velocity, mod.ProjectileType<AcidSpit>(), SpitDamage, SpitKnockback, projectile.owner);
            Main.PlaySound(4, projectile.position, 9);

            for (var i = 0; i < 4; i++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 39, velocity.X * 0.25f, velocity.Y * 0.25f);
            }
        }
    }
}
