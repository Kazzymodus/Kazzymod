using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Kazzymod.NPCs;

namespace Kazzymod.Buffs
{
    class Corroded : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Corroded");
            Description.SetDefault("Reduced defense");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<KazzyNPC>().corroded = true;
            
            var maxReduction = Math.Min(npc.defense, 5);
            if (maxReduction < 0) return;

            npc.defense -= maxReduction;
        }
    }
}
