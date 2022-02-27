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
    class MortarHydra : HydraHead
    {
        private const float MinLaunchSpeedY = 8;
        private const float MaxLaunchSpeedY = 13;
        private const float CrestHeight = 120; // If firing at NPCs above it, projectile should crest at least this high above them.
        private const float Gravity = 0.3f;

        protected override int AttackCooldown => 200;

        protected override void Attack()
        {
            // NPC target = Main.npc[Target];
            Player target = Main.player[projectile.owner];
            Vector2 direction = target.Center - projectile.Center;

            // So what this essentially does is take Gauss' formula for the sum of a series and solve it for n (which is time).
            // Could I do this with parabola? Perhaps, if I understood them.
            //
            // For those who like formulas:
            //
            // Gauss formula:                   d = t(2v + a(t - 1)) / 2
            // Solved for t:                    t = (-(2v - a) + sqrt((2v -a)^2 - 4a(-2d))) / (2 * a)
            // Solved for t (v = 0):            t = (-1 + sqrt(1 + 4(2d / a))) / 2
            // Solved for v:                    v = (-at^2 + at + 2d) / 2t
            //
            // d = distance
            // t = time
            // v = velocity (vertical)
            // a = acceleration (gravity)

            float yVelocity;
            float xVelocity;

            if (direction.Y > 0) // Target is below hydra.
            {
                yVelocity = MinLaunchSpeedY;

                var arcTime = 2 * (yVelocity / Gravity);

                var a = Gravity;
                var b = 2 * yVelocity - Gravity;
                float c = -2 * direction.Y;

                var fallTime = Quadratic(a, b, c);
                var totalTime = arcTime + fallTime;
                direction.X += totalTime * target.velocity.X;
                xVelocity = direction.X / totalTime;

            }
            else
            {
                // Doesn't take into account the Hydra's vertical velocity. Could fix that, but can't be arsed ATM.

                var crestDistance = direction.Y - CrestHeight;
                var crestTime = Quadratic(1, 1, crestDistance * 2 / Gravity);
                yVelocity = -((-Gravity * crestTime * crestTime + Gravity * crestTime + crestDistance * 2) / (2 * crestTime));

                if (yVelocity > MaxLaunchSpeedY)
                {
                    AbortAttack();
                    return;
                }

                var fallTime = Quadratic(1, 1, -CrestHeight * 2 / Gravity);

                var totalTime = crestTime + fallTime;
                direction.X += totalTime * target.velocity.X;
                xVelocity = direction.X / totalTime;
            }

            Vector2 velocity = new Vector2(xVelocity, -yVelocity);

            Projectile.NewProjectile(projectile.position, velocity, mod.ProjectileType<Mortar>(), 30, 10f, projectile.owner, Gravity);
            Main.PlaySound(4, projectile.position, 9);

            for (var i = 0; i < 4; i++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 39, velocity.X * 0.25f, velocity.Y * 0.25f);
            }
        }

        private float Quadratic(float a, float b, float c)
        {
            return (float)((-b + Math.Sqrt(b * b - 4 * a * c)) / (2f * a));
        }
    }
}
