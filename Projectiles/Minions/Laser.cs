using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Kazzymod.Projectiles.Minions
{
    class Laser : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.friendly = true;
            projectile.minion = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 180;
        }

        public override void AI()
        {
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {


            return false;
        }
    }
}
