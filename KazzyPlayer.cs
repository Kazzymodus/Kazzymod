using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using Kazzymod.Projectiles.Minions;

namespace Kazzymod
{
    class KazzyPlayer : ModPlayer
    {
        public bool yurneroDash;
        public List<NPC> yurneroVictims = new List<NPC>();

        private float dashTimer;
        private float dashDuration = 7f;

        private float bleedTimer;
        private float bleedDuration = 150f;

        public bool hydraMinion;
        public int hydraCount;

        public bool partyBombActive;
        public float bombStart;
        private float bombProgress;

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            return !yurneroDash;
        }

        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            if(yurneroDash)
            {
                if(!yurneroVictims.Contains(npc))
                    Main.PlaySound(SoundID.NPCHit, player.Center);
                
                yurneroVictims.Add(npc);
            }
        }

        public override void PostUpdate()
        {
            if (yurneroDash)
            {
                for(var i = 0; i < 2; i++)
                {
                    Dust.NewDust(player.Center, 0, 0, 16);
                }

                dashTimer++;

                if(dashTimer >= dashDuration)
                {
                    yurneroDash = false;
                    dashTimer = 0f;
                    player.velocity.X *= 0.1f;
                }
            }

            if(yurneroVictims.Count > 0)
            {
                bleedTimer++;

                if (bleedTimer % 5 == 0)
                {
                    foreach (NPC npc in yurneroVictims)
                    {
                        Dust.NewDust(npc.Center, 0, 0, 5);
                    }
                }

                if(bleedTimer >= bleedDuration)
                {
                    foreach(NPC npc in yurneroVictims)
                    {
                        for (var i = 0; i < 3; i++)
                        {
                            player.ApplyDamageToNPC(npc, 33, 0.5f, 0, true);
                        }
                    }

                    yurneroVictims.Clear();
                    bleedTimer = 0f;
                }
            }

            if (partyBombActive)
            {
                

                if (bombProgress < 260)
                {
                    bombProgress = Main.GlobalTime - bombStart;
                    Filters.Scene["Confetti"].GetShader().UseProgress(bombProgress * 0.725f);
                }

            }
        }

        private void UpdateHydra()
        {
            if (player.ownedProjectileCounts[mod.ProjectileType<HydraBody>()] > 0)
            {
                var hydraCount = 0;

                for (var i = 0; i < 1000; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].modProjectile is HydraHead)
                    {
                        hydraCount++;
                    }
                }

                this.hydraCount = hydraCount;

                hydraMinion = true;
            }
        }

        public override void ResetEffects()
        {
            hydraMinion = false;
            hydraCount = 0;
        }
    }
}
