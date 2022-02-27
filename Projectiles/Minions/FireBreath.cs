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
    class FireBreath : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.minion = true;
            projectile.friendly = true;
            projectile.timeLeft = 40; // Can't use Duration for this because AI is assigned after SetDefaults in called.
            projectile.penetrate = -1;
            projectile.alpha = 255;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Vector2 velocityDifference = projectile.velocity - oldVelocity;
            velocityDifference.X = Math.Abs(velocityDifference.X);
            velocityDifference.Y = Math.Abs(velocityDifference.Y);

            float totalVelocity = oldVelocity.Length();

            if (velocityDifference.X < velocityDifference.Y) // Vertical collision
            {
                projectile.velocity.X = (oldVelocity.X + Math.Sign(oldVelocity.X) * (totalVelocity - Math.Abs(oldVelocity.Y))) * 0.5f;
                projectile.velocity.Y = 0;
            }
            else if (velocityDifference.Y < velocityDifference.X) // Horizontal collision
            {
                projectile.velocity.Y = (oldVelocity.Y + Math.Sign(oldVelocity.Y) * (totalVelocity - Math.Abs(oldVelocity.X))) * 0.5f;
                projectile.velocity.X = 0;
            }

            return false;
        }

        public override void AI()
        {
            // Prevents half-blocks from acting up.
            Collision.StepUp(ref projectile.position, ref projectile.velocity, projectile.width, projectile.height, ref projectile.stepSpeed, ref projectile.gfxOffY);

            int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6, 0, 0, 0, default(Color), 4);
            Main.dust[dust].noGravity = true;

            dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6, 0, 0, 0, default(Color), 1.5f);
            Main.dust[dust].noLight = true;
        }
    }
}
