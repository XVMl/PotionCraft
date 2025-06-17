using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PotionCraft.Content.NPCs
{
    public class Thiu:ModNPC
    {
        public override string Texture => "PotionCraft/Assets/NPCs/Thiu";

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 25;
            NPCID.Sets.ActsLikeTownNPC[base.Type] = true;
            NPCID.Sets.ShimmerTownTransform[base.Type] = true;
            NPCID.Sets.TrailingMode[base.Type] = 3;
            NPCID.Sets.TrailCacheLength[base.Type] = 45;
            NPCID.Sets.DangerDetectRange[Type] = 1000;
            NPCID.Sets.AttackTime[Type] = 230;
            NPCID.Sets.AttackAverageChance[Type] = 0;
        }

        public override void SetDefaults()
        {
            NPC.dontTakeDamage = false;
            NPC.lifeMax = 20000;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.width = 10;
            NPC.height = 10;
            NPC.knockBackResist = 0;
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.lavaImmune = true;
            NPC.aiStyle = 7;
        }

        public override bool CheckDead()
        {
            NPC.life = 1;
            return false;
        }

        public override bool CanChat()
        {
            return base.CanChat();
        }

        public override string GetChat()
        {
            return base.GetChat();
        }


    }
}
