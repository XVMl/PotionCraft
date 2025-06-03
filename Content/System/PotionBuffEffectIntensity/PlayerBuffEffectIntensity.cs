using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PotionCraft.Content.System.PotionBuffEffectIntensity
{
    public class PlayerBuffEffectIntensity : ModPlayer
    {
        public Dictionary<int, int> BuffEffectIntensity = new() {
        {1,1},
        {2,1},
        {3,1},
        {4,1},
        {6,1},
        {7,1},
        {8,1},
        {9,1},
        {10,1},
        {11,1},
        {12,1},
        {13,1},
        {14,1},
        {15,1},
        {16,1},
        {17,1},
        {18,1},
        {48,1},
        {57,1},
        {58,1},
        {59,1},
        {62,1},
        {63,1},
        {71,1},
        {73,1},
        {74,1},
        {75,1},
        {76,1},
        {77,1},
        {78,1},
        {79,1},
        {93,1},
        {104,1},
        {105,1},
        {106,1},
        {107,1},
        {108,1},
        {109,1},
        {110,1},
        {111,1},
        {112,1},
        {113,1},
        {114,1},
        {115,1},
        {116,1},
        {117,1},
        {121,1},
        {122,1},
        {123,1}
        };

        public override void OnEnterWorld()
        {
            foreach (var item in BuffEffectIntensity.Keys)
            {
                BuffEffectIntensity[item] = 1;
            }
        }

        public override void UpdateDead()
        {
            foreach (var item in BuffEffectIntensity.Keys)
            {
                BuffEffectIntensity[item] = 1;
            }
        }

        public override void PostUpdate()
        {
        }

        public override void PostUpdateBuffs()
        {
            PlayerBuffEffectIntensity player = Main.LocalPlayer.GetModPlayer<PlayerBuffEffectIntensity>();
            if (Player.HasBuff(BuffID.Regeneration))
            {
                Player.lifeRegen += 2 * player.BuffEffectIntensity[BuffID.Regeneration]-2;
            }
            if (Player.HasBuff(BuffID.Swiftness))
            {
                Player.moveSpeed += 0.25f * player.BuffEffectIntensity[BuffID.Swiftness]-0.25f ;
            }
            if (Player.HasBuff(BuffID.Ironskin))
            {
                Player.statDefense += 8 * player.BuffEffectIntensity[BuffID.Ironskin] - 8;
            }
            if (Player.HasBuff(BuffID.ManaRegeneration))
            {
                Player.manaRegen += 2 * player.BuffEffectIntensity[BuffID.ManaRegeneration] - 2;
            }
            if (Player.HasBuff(BuffID.MagicPower))
            {
                Player.GetDamage<MagicDamageClass>() += 0.2f * player.BuffEffectIntensity[BuffID.Regeneration] - 0.2f;
            }
            if (Player.HasBuff(BuffID.Featherfall))
            {
                //Player.lifeRegen += 2 * player.BuffEffectIntensity[BuffID.Regeneration] - 2;
            }
            if (Player.HasBuff(BuffID.Spelunker))
            {
                //Player.lifeRegen += 2 * player.BuffEffectIntensity[BuffID.Regeneration] - 2;
            }
            if (Player.HasBuff(BuffID.Shine))
            {
                //Player.lifeRegen += 2 * player.BuffEffectIntensity[BuffID.Regeneration] - 2;
            }
            if (Player.HasBuff(BuffID.NightOwl))
            {
                //Player.lifeRegen += 2 * player.BuffEffectIntensity[BuffID.Regeneration] - 2;
            }
            if (Player.HasBuff(BuffID.Battle))
            {
                //Player.lifeRegen += 2 * player.BuffEffectIntensity[BuffID.Regeneration] - 2;
            }
            if (Player.HasBuff(BuffID.Thorns))
            {
                //Player.lifeRegen += 2 * player.BuffEffectIntensity[BuffID.Regeneration] - 2;
            }
            if (Player.HasBuff(BuffID.Archery))
            {
                Player.arrowDamage += 0.1f * player.BuffEffectIntensity[BuffID.Archery] - 0.1f;
            }
            if (Player.HasBuff(BuffID.Mining))
            {
                //Player.lifeRegen += 2 * player.BuffEffectIntensity[BuffID.Regeneration] - 2;
            }
            if (Player.HasBuff(BuffID.Heartreach))
            {
                //Player. += 2 * player.BuffEffectIntensity[BuffID.Regeneration] - 2;
            }
            if (Player.HasBuff(BuffID.AmmoBox))
            {
                //Player.ammoCost75 += 2 * player.BuffEffectIntensity[BuffID.Regeneration] - 2;
            }
            if (Player.HasBuff(BuffID.Calm))
            {
                //Player.lifeRegen += 2 * player.BuffEffectIntensity[BuffID.Regeneration] - 2;
            }
            if (Player.HasBuff(BuffID.Regeneration))
            {
                Player.lifeRegen += 2 * player.BuffEffectIntensity[BuffID.Regeneration] - 2;
            }
        }

    }
}
