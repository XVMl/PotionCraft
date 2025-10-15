using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.Items;
using PotionCraft.Content.System;
using Terraria;
using Terraria.UI;
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
                VAlign = 0.5f,
                HAlign = 0.5f,
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
            BasePotion createdPotion = CloneOrCreatPotion(PotionCraftState,potion);
            foreach (var buff in material.PotionDictionary)
            {
                createdPotion.PotionDictionary.TryAdd(buff.Key,buff.Value);
                createdPotion.PotionDictionary[buff.Key].BuffTime+= buff.Value.BuffTime;
                createdPotion.PotionDictionary[buff.Key].Counts+= buff.Value.Counts;
            }
            for (int i = 0; i < material.DrawCountList.Count; i++)
            {
                createdPotion.DrawPotionList.Add(material.DrawPotionList[i]);
                createdPotion.DrawCountList.Add(material.DrawCountList[i]);
            }
            createdPotion.MashUpCount += material.MashUpCount+1;
            createdPotion._Name +=
                $"{material._Name} +";    
            if (createdPotion.MashUpCount == material.MashUpCount + 1)
                createdPotion._Name = material._Name;
        }

        private void MashUp(Item potion, Item material)
        {
            BasePotion createdPotion = CloneOrCreatPotion(PotionCraftState,potion);
            var name = Lang.GetBuffName(material.buffType);
            createdPotion.PotionDictionary.TryAdd(name, new PotionData(
                name,
                material.type,
                0,
                0,
                material.buffType
            ));
            createdPotion.PotionDictionary[name].BuffTime+= material.buffTime;
            createdPotion.PotionDictionary[name].Counts++;
            createdPotion.DrawPotionList.Add(material.type);
            createdPotion.DrawCountList.Add(1);
            createdPotion._Name +=
                $"{Lang.GetBuffName(material.buffType).Replace(" ", "")} + ";    
            if (createdPotion.MashUpCount == 0)
                createdPotion._Name = $"{Lang.GetBuffName(material.buffType).Replace(" ","")} ";
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
            PotionCraftState.Potion.stack--;
            PotionCraftState.Material.stack--;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.UI.Button, GetDimensions().ToRectangle(), Color.White);
        }

    }
}

