using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Kazzymod.NPCs
{
    class KazzyNPC : GlobalNPC
    {
        public bool corroded;

        public override bool InstancePerEntity => true;

        public override void ResetEffects(NPC npc)
        {
            corroded = false;
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (corroded)
            {
                drawColor.R = (byte)(drawColor.R * 0.5f);
                drawColor.B = (byte)(drawColor.B * 0.65f);
                
                if (Main.rand.Next(20) == 0)
                {
                    int dust = Dust.NewDust(npc.position, npc.width, npc.height, 46, 0f, 0f, 120, default(Color), 0.2f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].fadeIn = 1.9f;
                }
            }
        }
    }
}
