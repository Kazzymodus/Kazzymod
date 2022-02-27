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
    class FireHydra : HydraHead
    {
        private const int AttackSpeed = 4;
        private const float FireSpeed = 10f;
        private const int FireDamage = 5;
        private const int EndRadius = 50;
        private const int FlameDuration = 40;

        protected override TargetMode TargetPriority => TargetMode.FurthestFromHydra;

        protected override int AttackCooldown => 150;

        protected override int AttackDuration => 80;

        protected override float MaxAttackRange => 500;

        protected override bool ContinueAttackWhenTargetDead => true;

        private Vector2 lastPosition;

        protected override void Attack()
        {
            Vector2 targetPosition = HasTarget ? Main.npc[Target].Center : lastPosition;

            if (Math.Abs(Timer) % 8 == 0)
            {
                Vector2 direction = targetPosition - projectile.Center;
                projectile.direction = Math.Sign(direction.X);
                Vector2 velocity = Main.projectile[BodyId].velocity + Vector2.Normalize(direction) * FireSpeed;

                Projectile.NewProjectile(projectile.position, velocity, mod.ProjectileType<FireBreath>(), FireDamage, 0, projectile.owner);

                for (var i = 0; i < 2; i++)
                {
                    int dustId = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6, 4 * projectile.direction, 0, 0, default(Color), 3f);
                    Main.dust[dustId].noLight = true;
                }
            }

            lastPosition = targetPosition;
        }
    }
}
