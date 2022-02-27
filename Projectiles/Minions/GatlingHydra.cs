using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Kazzymod.Projectiles.Minions
{
    class GatlingHydra : HydraHead
    {
        private const int WindUp = 120;
        private const int AttackSpeed = 6;
        private const int BulletSpeed = 10;
        private const int BulletDamage = 20;
        private const float BulletKnockback = 4f;
        private const float Spread = 0.1f;
        private const int LeadTime = 3;

        protected override int AttackDuration => 300;
        protected override int AttackCooldown => 300;
        protected override bool ContinueAttackWhenTargetDead => true;

        private Vector2 lastPosition;

        protected override void PreAttack()
        {
            Main.PlaySound(SoundID.Camera, projectile.position); // Temp
        }

        protected override void Attack()
        {
            var absTimer = Math.Abs(Timer);
            
            if (absTimer < WindUp && (!HasTarget || !Main.npc[Target].active))
            {
                AbortAttack();
                return;
            }
            else if (absTimer >= WindUp && absTimer % AttackSpeed == 0)
            {
                Vector2 targetPosition;

                if (HasTarget)
                {
                    NPC target = Main.npc[Target];

                    if (!target.active)
                    {
                        ClearTarget();
                        targetPosition = lastPosition;
                    }
                    else
                    {
                        targetPosition = target.Center + target.velocity * LeadTime;
                    }
                }
                else
                {
                    targetPosition = lastPosition;
                }

                Vector2 direction = Vector2.Normalize(targetPosition - projectile.position);
                Vector2 velocity = (direction * BulletSpeed).RotatedByRandom(0.05f);

                Projectile.NewProjectile(projectile.position, velocity, ProjectileID.Bullet, BulletDamage, BulletKnockback, projectile.owner);
                lastPosition = targetPosition;
            }
        }
    }
}
