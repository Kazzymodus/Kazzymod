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
    class HornetMissile : ModProjectile
    {
        private const int ExplosionRadius = 100;
        private const float MoveSpeed = 10;
        private const float TurnSpeed = 0.05f;
        private const int Duration = 180;
        private const int ArmDelay = 40; // Could've called this PrimeTime but there you are.
        private const float DragFactor = 0.975f;

        private int Target
        {
            get { return (int)projectile.ai[0]; }
        }
        
        private bool HasTarget
        {
            get { return Target >= 0; }
        }

        private bool HasExploded
        {
            get { return projectile.ai[1] == 1; }
            set { projectile.ai[1] = value ? 1 : 0; }
        }

        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 24;
            projectile.minion = true;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.timeLeft = Duration;
        }

        public override void AI()
        {
            if (projectile.timeLeft <= 3)
            {
                if (!HasExploded)
                {
                    Explode();
                }

                return;
            }

            projectile.rotation = projectile.velocity.ToRotation();

            Dust trail = Dust.NewDustDirect(projectile.position - projectile.velocity, projectile.width, projectile.height, 31, 0, 0, 0, default(Color), 0.4f);
            trail.velocity = Vector2.Zero;
            trail.noGravity = true;
            trail.fadeIn = 1.4f;
            trail = Dust.NewDustDirect(projectile.position - projectile.velocity, projectile.width, projectile.height, 6, 0, 0, 0, default(Color), 0.7f);
            trail.noGravity = true;
            trail.fadeIn = 1f;

            if (projectile.timeLeft > Duration - ArmDelay)
            {
                projectile.velocity *= DragFactor;
            }
            else
            {
                if (Target >= 0 && !Main.npc[Target].active)
                {
                    ClearTarget();
                }

                if (Target >= 0)
                {
                    NPC target = Main.npc[Target];
                    
                    Vector2 targetDirection = Vector2.Normalize(target.Center - projectile.Center);
                    var targetAngle = (float)Math.Atan2(targetDirection.Y, targetDirection.X);
                    var angleDifference = targetAngle - projectile.rotation;

                    if (angleDifference > Math.PI)
                    {
                        angleDifference -= (float)(2 * Math.PI);
                    }
                    else if (angleDifference < -Math.PI)
                    {
                        angleDifference += (float)(2 * Math.PI);
                    }

                    projectile.rotation += angleDifference * TurnSpeed;
                }
                
                projectile.velocity = Utils.ToRotationVector2(projectile.rotation) * MoveSpeed;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!HasExploded)
            {
                Explode();
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (!HasExploded)
            {
                Explode();
            }
            
            return false;
        }

        private void Explode()
        {
            projectile.timeLeft = 3;
            projectile.alpha = 255;
            projectile.velocity = Vector2.Zero;

            for (var i = 0; i < 15; i++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 31);
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 55);
            }

            Vector2 explosionSize = new Vector2(ExplosionRadius);
            projectile.position -= explosionSize * 0.5f;
            projectile.Size = explosionSize;
            
            Gore.NewGore(projectile.Center, -Vector2.UnitY, Main.rand.Next(11, 14));
            Main.PlaySound(2, projectile.Center, 14);

            HasExploded = true;
        }

        private void ClearTarget()
        {
            projectile.ai[0] = -1;
        }
    }
}
