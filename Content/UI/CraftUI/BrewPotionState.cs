using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.Items;
using PotionCraft.Content.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria;
using Microsoft.Xna.Framework;
using static PotionCraft.Content.System.AutoLoaderSystem.LoaderPotionOrMaterial;
using static PotionCraft.Assets;
namespace PotionCraft.Content.UI.CraftUI
{
    public class BrewPotionState : AutoUIState
    {
        public override bool IsLoaded() => ActiveState && CraftState == CraftUiState.BrewPotion;

        public override string LayersFindIndex => "Vanilla: Mouse Text";

        private PotionSlot<BrewPotionState> PotionSlot;

        private BrewPotionButton BrewPotionButton;

        public int PotionCount = 20;

        private BasePotion CreatPotion = ModContent.GetInstance<BasePotion>();

        private UIElement Area;
        public override void OnInitialize()
        {

            Width.Set(920, 0);
            Height.Set(640, 0);
            HAlign = 0.5f;
            VAlign = 0.5f;
            Area = new UIElement()
            {
                HAlign = 0.2f,
                VAlign = 0.3f,
            };
            Area.Width.Set(270f, 0);
            Area.Height.Set(270f, 0);
            Append(Area);

            PotionSlot = new(this)
            {
                HAlign = 0.5f,
                VAlign = 0.5f,
            };
            Area.Append(PotionSlot);

            BrewPotionButton = new BrewPotionButton(this)
            {
                HAlign = 0.5f,
                VAlign = 1f,
            };
            Append(BrewPotionButton);

        }

        public static bool QuicklyCheckPotion(Item item1,BasePotion item2)
        {
            if (item1.ModItem is not BasePotion)
                return false;
            return AsPotion(item1)._Name == item2._Name;
        }
        
        private void AddPotion(Item item)
        {
            if (ModPotionList.ContainsKey(item)&&item.ModItem is not BasePotion)
                return;
            
            if (!QuicklyCheckPotion(item, CreatPotion) && item.type != ModContent.ItemType<MagicPanacea>() )
            {
                MashUp(item);
                return; 
            }
            
            foreach (var buff in CreatPotion.PotionDictionary)
            {
                buff.Value.BuffTime *= 2;
                buff.Value.Counts *= 2;
            }
            for (var i = 0; i < CreatPotion.DrawPotionList.Count; i++)
            {
                CreatPotion.DrawCountList[i] *= 2;
            }
            CreatPotion.PurifyingCount++;
            CreatPotion._Name += "@ ";
        }
        
        private void MashUp(Item item)
        {
            var count = 0;
            if(item.ModItem is BasePotion)
                goto BasePotion;
            
            var name = Lang.GetBuffName(item.buffType);
            CreatPotion.PotionDictionary.TryAdd(name, new PotionData(
                name,
                item.type,
                0,
                0,
                item.buffType
            ));
            CreatPotion.PotionDictionary[name].BuffTime+= item.buffTime;
            CreatPotion.PotionDictionary[name].Counts++;
            CreatPotion.DrawPotionList.Add(item.type);
            CreatPotion.DrawCountList.Add(1);
            CreatPotion._Name +=
                $"{Lang.GetBuffName(item.buffType).Replace(" ", "")} + ";    
            goto End;
            
            BasePotion:
            var material = AsPotion(item);
            foreach (var buff in material.PotionDictionary)
            {
                CreatPotion.PotionDictionary.TryAdd(buff.Key,buff.Value);
                CreatPotion.PotionDictionary[buff.Key].BuffTime+= buff.Value.BuffTime;
                CreatPotion.PotionDictionary[buff.Key].Counts+= buff.Value.Counts;
            }
            for (int i = 0; i < material.DrawCountList.Count; i++)
            {
                CreatPotion.DrawPotionList.Add(material.DrawPotionList[i]);
                CreatPotion.DrawCountList.Add(material.DrawCountList[i]);
            }
            CreatPotion.MashUpCount += material.MashUpCount+1;
            count += material.MashUpCount;
            CreatPotion._Name +=
                $"{material._Name} +";    
            
            End:
            CreatPotion.MashUpCount++; 
            if (CreatPotion.MashUpCount == 0|| CreatPotion.MashUpCount == count + 1)
                CreatPotion._Name = $"{Lang.GetBuffName(item.buffType).Replace(" ","")} ";
        }
        
        private void ClearAll()
        {
            CreatPotion = ModContent.GetInstance<BasePotion>();
        }
        
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            //spriteBatch.Draw(UITexture("PotionCraftBG").Value, GetDimensions().ToRectangle(), Color.White);
        }

    }

    public class BrewPotionButton : PotionElement<BrewPotionState>
    {

        public BrewPotionButton(BrewPotionState brewPotionState)
        {
            PotionCraftState = brewPotionState;
            Width.Set(100f, 0);
            Height.Set(32f, 0);
        }

        private void BrewPotion(Item potion)
        {
            if (!IsPotion(potion)) return;
            PotionCraftState.Potion.stack = PotionCraftState.PotionCount;
            Main.LocalPlayer.BuyItem(50 * PotionCraftState.PotionCount);
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
            if (PotionCraftState.Potion.IsAir) return;
            BrewPotion((PotionCraftState.Potion));
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            var rectangle = GetDimensions().ToRectangle();
            spriteBatch.Draw(Assets.UI.Button, rectangle, Color.White);
            ItemSlot.DrawSavings(spriteBatch,rectangle.X, rectangle.Y);
        }

    }
}
