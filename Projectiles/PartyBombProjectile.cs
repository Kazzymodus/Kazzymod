using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kazzymod.Projectiles
{
    public class PartyBombProjectile : ModProjectile
    {
        private const int pulseCount = 3;
        private const int rippleSize = 15;
        private const int speed = 45;
        private const float gravity = 0.15f;
        private const float vibrationScalarMax = 12f;
        private int dustType = 61;

        private float rotationSpeed = 0.03f;

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 24;
            projectile.timeLeft = 420;
            projectile.penetrate = -1;
        }

        public override void AI()
        {
            //projectile.velocity.X = (float)(Math.Cos((420 - projectile.timeLeft) * 0.4f) * 8 * -projectile.ai[1]);
            //projectile.velocity.Y = (float)(Math.Cos((420 - projectile.timeLeft) * 0.4f) * 8 * projectile.ai[0]);

            //projectile.velocity += new Vector2(projectile.ai[0], projectile.ai[1]) * 10;

            if (projectile.timeLeft <= 180)
            {
                if (projectile.ai[0] != 2)
                {
                    projectile.ai[0] = 2;
                    projectile.alpha = 255;

                    if (!Filters.Scene["HydraShockwave"].IsActive())
                    {
                        Filters.Scene.Activate("HydraShockwave", projectile.Center).GetShader().UseColor(pulseCount, rippleSize, speed).UseTargetPosition(projectile.Center);
                    }

                    if (!Filters.Scene["Confetti"].IsActive())
                    {
                        Filters.Scene.Activate("Confetti", projectile.Center).GetShader().UseImage("Images/Misc/noise", 0, null).UseTargetPosition(projectile.Center);
                        Main.player[projectile.owner].GetModPlayer<KazzyPlayer>().bombStart = Main.GlobalTime;
                    }

                    Main.player[projectile.owner].GetModPlayer<KazzyPlayer>().partyBombActive = true;

                    Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 62, 1f, 0f);
                    Main.PlaySound(4, (int)projectile.position.X, (int)projectile.position.Y, 58, 1, -0.4f);

                    Vector2 velocity = Utils.ToRotationVector2(projectile.rotation);

                    for (var i = 0; i < 40; i++)
                    {
                        Vector2 dustVelocity = velocity * Main.rand.NextFloat();

                        if (i % 2 == 0)
                        {
                            dustVelocity = -dustVelocity;
                        }

                        Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, dustType, velocity.X, velocity.Y, 0, default(Color), 4f);
                        dust.velocity = dustVelocity * 40f;
                        dust.noGravity = true;
                    }

                    Vector2 goreVelocity = Utils.RotatedBy(velocity, 0.25 * Math.PI);

                    for (var i = 0; i < 4; i++)
                    {
                        goreVelocity = new Vector2(-goreVelocity.Y, goreVelocity.X);

                        Gore gore = Gore.NewGoreDirect(projectile.Center, Vector2.Zero, Main.rand.Next(61, 64), 2f);
                        gore.velocity = goreVelocity * 10;
                    }
                }

                var progress = (180f - projectile.timeLeft) / 60f;
                Filters.Scene["HydraShockwave"].GetShader().UseProgress(progress).UseOpacity(100f);

                return;
            }
            if (projectile.timeLeft <= 330)
            {
                if (projectile.ai[0] == 0)
                {
                    projectile.ai[0] = 1;
                    projectile.frame++;
                    Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 61, 1, 0.7f);
                    rotationSpeed *= 0.5f;
                }

                Lighting.AddLight(projectile.Center, new Vector3(0.5f, 1, 0.5f));

                projectile.velocity *= 0.9f;

                var vibrationScalar = (1 - (projectile.timeLeft - 180) / 150f) * vibrationScalarMax;

                var vibrationX = Main.rand.NextFloat() * vibrationScalar;
                var direction = ((projectile.timeLeft % 2) - 0.5f) * 2;
                projectile.position += new Vector2(vibrationX * direction, 0);

                if (Main.rand.NextFloat() > (projectile.timeLeft - 180) / 150f)
                {
                    float xPos;
                    float yPos;

                    do
                    {
                        xPos = Main.rand.Next(-32, 33);
                        yPos = Main.rand.Next(-32, 33);
                    }
                    while (Math.Abs(xPos) + Math.Abs(yPos) < 16f);

                    //Vector2 focalPoint = projectile.position;
                    //focalPoint.X += projectile

                    Vector2 spawnPos = projectile.Center + projectile.velocity * 14 + new Vector2(xPos, yPos);
                    Vector2 velocity = (projectile.Center + projectile.velocity * 14 - spawnPos) * 0.075f;

                    Dust dust = Dust.NewDustPerfect(spawnPos, dustType, velocity, 0, default(Color), 2);
                    dust.noGravity = true;
                }
            }
            else
            {
                Lighting.AddLight(projectile.Center, new Vector3(0.5f, 1, 0.5f));

                if (projectile.timeLeft % 4 == 0)
                {
                    Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType, 0, 0, 0, default(Color), 2f);
                }
            }

            projectile.rotation += (float)(Math.PI * rotationSpeed) * projectile.direction;
            projectile.velocity.Y += gravity;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void Kill(int timeLeft)
        {
            Filters.Scene["HydraShockwave"].Deactivate();
        }
    }
}
