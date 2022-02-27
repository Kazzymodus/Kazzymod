using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace Kazzymod.Projectiles.Minions
{
    class RockHydra : HydraHead
    {
        private const int ShockwaveMaxDamage = 30;
        private const int ShockwaveMaxKnockback = 30;
        private const int ShockwaveDamageTickRate = 20;
        private const int EpicenterRadius = 175;

        private const int RippleAmount = 3;
        private const int RippleSize = 20;
        private const int RippleSpeed = 40;
        private const int RippleDistort = 100;

        protected override float MaxAttackRange => 400;
        protected override int AttackCooldown => 240;
        protected override int AttackDuration => 90;

        protected override bool ContinueAttackWhenTargetDead => true;

        protected override void Attack()
        {
            var absTimer = Math.Abs(Timer);

            if ((absTimer - 1) % ShockwaveDamageTickRate == 0)
            {
                var minimumReachSq = EpicenterRadius * EpicenterRadius;
                var reachSq = minimumReachSq + (MaxAttackRange * MaxAttackRange - minimumReachSq) *
                    (absTimer / (float)AttackDuration);

                for (var i = 0; i < 200; i++)
                {
                    var npc = Main.npc[i];

                    if (!npc.active || npc.friendly ||
                        !Collision.CanHit(projectile.position, projectile.width, projectile.height,
                            npc.position, npc.width, npc.height)) continue;

                    var hydraToNpc = npc.Center - projectile.Center;

                    if (hydraToNpc.X * hydraToNpc.X + hydraToNpc.Y * hydraToNpc.Y < reachSq)
                    {
                        var distance = (float)Math.Sqrt(hydraToNpc.X * hydraToNpc.X + hydraToNpc.Y * hydraToNpc.Y);
                        var intensity = 1 - (distance / MaxAttackRange);
                        npc.velocity += hydraToNpc / distance * ShockwaveMaxKnockback * intensity * npc.knockBackResist;

                        if (!npc.dontTakeDamage && !npc.immortal)
                        {
                            npc.StrikeNPC((int)(ShockwaveMaxDamage * intensity), 0, 0);
                        }
                    }
                }
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (Timer >= 0) return;

            var absTimer = Math.Abs(Timer);

            if (!Filters.Scene["HydraShockwave"].IsActive())
            {
                Filters.Scene.Activate("HydraShockwave", projectile.position)
                    .GetShader().UseColor(RippleAmount, RippleSize, RippleSpeed)
                    .UseTargetPosition(projectile.position);
            }

            var progress = absTimer / (float)AttackDuration;
            Filters.Scene["HydraShockwave"].GetShader().UseProgress(progress)
                .UseOpacity(RippleDistort * (1 - progress));
        }

        public override void Kill(int timeLeft)
        {
            Filters.Scene["HydraShockwave"].Deactivate();
        }
    }
}