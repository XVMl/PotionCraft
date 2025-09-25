using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.Items;
using PotionCraft.Content.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using static PotionCraft.Content.System.LanguageHelper;
using static PotionCraft.Assets;
namespace PotionCraft.Content.UI.CraftUI
{
    public class MashUpState : AutoUIState
    {
        public override bool IsLoaded() => ActiveState && CraftState == CraftUiState.MashUp;

        public override string LayersFindIndex => "Vanilla: Mouse Text";

        private PotionSlot<MashUpState> PotionSlot;

        private MaterialSlot<MashUpState> MaterialSlot;

        private CreatedPotionSlot<MashUpState> CreatedPotionSlot;

        private MashUpButton mashupbutton;

        private UIElement area;

        public override void OnInitialize()
        {
            Width.Set(920, 0);
            Height.Set(640, 0);
            HAlign = 0.5f;
            VAlign = 0.5f;
            area = new UIElement()
            {
                HAlign = 0.2f,
                VAlign = 0.3f,
            };
            area.Width.Set(270f, 0);
            area.Height.Set(270f, 0);
            Append(area);
            PotionSlot = new(this)
            {
                HAlign = 0.1f,
                VAlign = 0.0f,
                TexturePath = "ItemSlot"
            };
            area.Append(PotionSlot);

            CreatedPotionSlot = new(this)
            {
                HAlign = 0.05f,
                VAlign = 0.5f,
            };
            area.Append(CreatedPotionSlot);

            MaterialSlot = new(this)
            {
                HAlign = 0.6f,
                VAlign = 0.0f,
                TexturePath = "MaterialSlot"
            };
            area.Append(MaterialSlot);

            mashupbutton = new(this)
            {
                VAlign = 0.0f,
                HAlign = 0.0f,
            };
            Append(mashupbutton);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(UITexture("PotionCraftBG").Value, GetDimensions().ToRectangle(), Color.White);
            spriteBatch.Draw(UITexture("MashUpBG").Value, area.GetDimensions().ToRectangle(), Color.White);
            base.Draw(spriteBatch);
        }

        

    }

    public class MashUpButton : PotionElement<MashUpState>
    {
        public MashUpButton(MashUpState mashUpState)
        {
            PotionCraftState = mashUpState;
            Width.Set(100f, 0);
            Height.Set(32f, 0);
        }
        private void MashUp(Item potion, BasePotion material)
        {
            BasePotion createdPotion;
            if (IsMaterial(potion) && Main.LocalPlayer.GetModPlayer<PotionCraftModPlayer>().CanNOBasePotion)
            {

                Item item = new();
                item.SetDefaults(ModContent.GetInstance<BasePotion>().Type);
                PotionCraftState.CreatedPotion = item.Clone();
                createdPotion = AsPotion(PotionCraftState.CreatedPotion);
                createdPotion.DrawPotionList.Add(potion.type);
                createdPotion.DrawCountList.Add(1);
                createdPotion.PotionDictionary.TryAdd(potion.buffType, new PotionData(
                    potion.buffType,
                    potion.type,
                    0,
                    potion.buffTime
                ));
            }
            else
            {
                PotionCraftState.CreatedPotion = potion.Clone();
                createdPotion = AsPotion(PotionCraftState.CreatedPotion);
                createdPotion.PotionDictionary = createdPotion.PotionDictionary.ToDictionary(k => k.Key, v => v.Value);
                createdPotion.DrawPotionList = [.. createdPotion.DrawPotionList];
                createdPotion.DrawCountList = [.. createdPotion.DrawCountList];
            }
            createdPotion.PotionDictionary = createdPotion.PotionDictionary.ToDictionary(k => k.Key, v => v.Value);
            foreach (var buff in material.PotionDictionary)
            {
                createdPotion.PotionDictionary.TryAdd(buff.Key, new PotionData(
                    buff.Value.BuffId,
                    buff.Value.ItemId,
                    1,
                    buff.Value.BuffTime
                ));
                createdPotion.PotionDictionary[buff.Key].BuffTime+= buff.Value.BuffTime;
                createdPotion.PotionDictionary[buff.Key].Counts+= buff.Value.Counts;
                createdPotion.DrawPotionList.Add(buff.Key);
                createdPotion.DrawCountList.Add(buff.Value.Counts);
            }
            createdPotion.MashUpCount+=material.MashUpCount;
            createdPotion.PotionName += $"{TryGetMashUpText(Math.Min(14, createdPotion.MashUpCount))} {material.DisplayName.Value} ";
        }

        private void MashUp(Item potion, Item material)
        {
            BasePotion createdPotion;
            if (IsMaterial(potion) && Main.LocalPlayer.GetModPlayer<PotionCraftModPlayer>().CanNOBasePotion)
            {
                Item item = new();
                item.SetDefaults(ModContent.GetInstance<BasePotion>().Type);
                PotionCraftState.CreatedPotion = item.Clone();
                createdPotion = AsPotion(PotionCraftState.CreatedPotion);
                createdPotion.DrawPotionList.Add(potion.type);
                createdPotion.DrawCountList.Add(1);
                createdPotion.PotionDictionary.TryAdd(potion.buffType, new PotionData(
                    potion.buffType,
                    potion.type,
                    0,
                    potion.buffTime
                ));
            }
            else
            {
                PotionCraftState.CreatedPotion = potion.Clone();
                createdPotion = AsPotion(PotionCraftState.CreatedPotion);
                createdPotion.PotionDictionary = createdPotion.PotionDictionary.ToDictionary(k => k.Key, v => v.Value);
                createdPotion.DrawPotionList = [.. createdPotion.DrawPotionList];
                createdPotion.DrawCountList = [.. createdPotion.DrawCountList];

            }
            createdPotion.PotionDictionary.TryAdd(material.buffType, new PotionData(
                material.buffType,
                 material.type,
                 0,
                 material.buffTime
            ));
            createdPotion.PotionDictionary[material.buffType].BuffTime+= material.buffTime;
            createdPotion.PotionDictionary[material.buffType].Counts++;
            createdPotion.DrawPotionList.Add(material.type);
            createdPotion.DrawCountList.Add(1);
            createdPotion.PotionName += $"{TryGetMashUpText(Math.Min(14, createdPotion.MashUpCount))}{TryGetPotionText(material.buffType)} ";
            createdPotion.MashUpCount++;
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
            if (PotionCraftState.Potion.IsAir || PotionCraftState.Material.IsAir) return;
            if (IsMaterial(PotionCraftState.Material))
            {
                MashUp(PotionCraftState.Potion, PotionCraftState.Material);
            }
            else
            {
                MashUp(PotionCraftState.Potion, AsPotion(PotionCraftState.Material));
            }

        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.UI.Button, GetDimensions().ToRectangle(), Color.White);
        }

    }
}

