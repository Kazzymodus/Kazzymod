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
    public class HornetHydra : HydraHead 
    {
        private const int MissileDamage = 50;
        private const float MissileKnockback = 5f;
        private const int FireRate = 8;
        private const float LaunchSpeedY = 15;
        private const float LaunchSpeedX = 4;

        protected override int AttackDuration => 240;
        protected override int AttackCooldown => 600;

        protected override bool ContinueAttackWhenTargetDead => true;

        protected override void Attack()
        {
            var absTimer = Math.Abs(Timer);

            if (Timer % FireRate == 0)
            {
                if (!GetRandomTarget(Main.player[projectile.owner]))
                {
                    ClearTarget();
                }

                Vector2 launchVelocity = new Vector2(Main.rand.NextFloat(-LaunchSpeedX, LaunchSpeedX), -LaunchSpeedY); // NextFloat is maximum value exclusive and that bugs the hell out of me.

                Projectile.NewProjectile(projectile.position, launchVelocity, mod.ProjectileType<HornetMissile>(), MissileDamage, MissileKnockback, projectile.owner, Target);
                Main.PlaySound(SoundID.Item42, projectile.position);
            }            
        }
    }
}
