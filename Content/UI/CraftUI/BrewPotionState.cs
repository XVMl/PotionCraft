using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.Items;
using PotionCraft.Content.System;
using System;
using System.Reflection;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.GameContent;
using Terraria.UI.Chat;
using static PotionCraft.Content.System.AutoLoaderSystem.LoaderPotionOrMaterial;
namespace PotionCraft.Content.UI.CraftUI
{
    public class BrewPotionState : AutoUIState
    {
        public override bool Isload() => true;

        public static readonly MethodInfo SetModItem = typeof(Item).GetProperty(nameof(Item.ModItem))!.GetSetMethod(true);
        
        public override string LayersFindIndex => "Vanilla: Mouse Text";
        
        public Item currentItem;

        public Item PreviewPotion;
        
        public Item PotionMaterial;

        public BasePotion CreatPotion = ModContent.GetInstance<BasePotion>();
        
        public BaseCustomMaterials CustomMaterials =  ModContent.GetInstance<BaseCustomMaterials>();
        
        public Action<BasePotion,Item> Craft;
        
        private PotionSynopsis PotionSynopsis;

        private PotionComponent potionComponent;

        private PotionSetting potionSetting;

        private PotionCrucible potionCrucible;

        //private PotionBaseSelect potionBaseSelect;

        public override void OnInitialize()
        {
            potionCrucible = new PotionCrucible(this);
            potionCrucible.HAlign = .5f;
            potionCrucible.Top.Set(200f, 0);
            Append(potionCrucible);

            potionSetting = new PotionSetting(this)
            {
                HAlign = .5f,
                VAlign = .9f
            };
            Append(potionSetting);

            PotionSynopsis = new PotionSynopsis(this);
            PotionSynopsis.Top.Set(320, 0);
            PotionSynopsis.Left.Set(370, 0);
            //PotionSynopsis.IdelAnimation = () =>
            //{
            //    PotionSynopsis.Top.Set(PotionSynopsis.Top.Pixels +
            //        (float)(.51 * Math.Sin(Math.PI / 150 * Main.time)), 0);
            //};
            //PotionSynopsis.SourcePotion = new Vector2(PotionSynopsis.Left.Pixels, PotionSynopsis.Top.Pixels);
            Append(PotionSynopsis);

            potionComponent = new PotionComponent(this);
            potionComponent.Top.Set(320, 0);
            potionComponent.Left.Set(1150, 0);
            //potionComponent.IdelAnimation = () =>
            //{
            //    potionComponent.Top.Set(potionComponent.Top.Pixels +
            //        (float)(.41 * Math.Sin(Math.PI / 154 * Main.time)), 0);
            //};
            Append(potionComponent);

            
        }

        #region 操作具体方法

        public static bool QuicklyCheckPotion(Item item1,BasePotion item2)
        {
            if (item1.ModItem is not BasePotion)
                return false;
            return AsPotion(item1)._Name == item2._Name;
        }
        
        public void AddPotion(Item item)
        {
            if(CreatPotion is not null && currentItem is not null)
                Craft?.Invoke(CreatPotion,currentItem);

            potionCrucible.Putity.Active = false;
            potionCrucible.MashUp.Active = false;
            currentItem = item.Clone();
            if(item.type != ModContent.ItemType<MagicPanacea>())
            {
                Craft = MashUp;
                potionCrucible.MashUp.Active = true;
            }
            if (item.type == ModContent.ItemType<MagicPanacea>() || QuicklyCheckPotion(item, CreatPotion))
            {
                Craft = Putify;
                potionCrucible.Putity.Active = true;
            }
            PotionMaterial = new Item();
            PotionMaterial.SetDefaults(ModContent.ItemType<BasePotion>());

            Craft.Invoke(AsPotion(PotionMaterial), currentItem);

            Refresh();
        }
        
        public void Putify(BasePotion potion,Item item)
        {
            foreach (var buff in potion.PotionDictionary)
            {
                buff.Value.BuffTime *= 2;
                buff.Value.Counts *= 2;
            }
            for (var i = 0; i < potion.DrawPotionList.Count; i++)
            {
                potion.DrawCountList[i] *= 2;
            }
            potion.PurifyingCount++;
            potion._Name += "@ ";
        }

        public void MashUp(BasePotion potion,Item item)
        {
            var count = 0;
            if(item.ModItem is BasePotion)
                goto BasePotion;
            
            var name = Lang.GetBuffName(item.buffType);
            potion.PotionDictionary.TryAdd(name, new PotionData(
                name,
                item.type,
                0,
                0,
                item.buffType
            ));
            potion.PotionDictionary[name].BuffTime+= item.buffTime;
            potion.PotionDictionary[name].Counts++;
            potion.DrawPotionList.Add(item.type);
            potion.DrawCountList.Add(1);
            potion._Name +=
                $"{BuffID.Search.GetName(item.buffType).Replace(" ", "")} + ";    
            goto End;
            
            BasePotion:
            var material = AsPotion(item);
            foreach (var buff in material.PotionDictionary)
            {
                potion.PotionDictionary.TryAdd(buff.Key,buff.Value);
                potion.PotionDictionary[buff.Key].BuffTime+= buff.Value.BuffTime;
                potion.PotionDictionary[buff.Key].Counts+= buff.Value.Counts;
            }
            
            for (var i = 0; i < material.DrawCountList.Count; i++)
            {
                potion.DrawPotionList.Add(material.DrawPotionList[i]);
                potion.DrawCountList.Add(material.DrawCountList[i]);
            }
            potion.MashUpCount += material.MashUpCount+1;
            count += material.MashUpCount;
            potion._Name +=
                $"{material._Name} +";    
            
            End:
            potion.MashUpCount++;
            item.stack--;
            if (potion.MashUpCount == 0|| potion.MashUpCount == count + 1)
                potion._Name = $"{BuffID.Search.GetName(item.buffType).Replace(" ","")} ";
        }

        #endregion
        
        public static void DrawCost(SpriteBatch spriteBatch,int cost,Vector2 pos)
        {
            var text2 = "";
            var num59 = 0;
            var num60 = 0;
            var num61 = 0;
            var num62 = 0;
            var num63 = cost;
            if (num63 < 1)
                num63 = 1;

            if (num63 >= 1000000)
            {
                num59 = num63 / 1000000;
                num63 -= num59 * 1000000;
            }

            if (num63 >= 10000)
            {
                num60 = num63 / 10000;
                num63 -= num60 * 10000;
            }

            if (num63 >= 100)
            {
                num61 = num63 / 100;
                num63 -= num61 * 100;
            }

            if (num63 >= 1)
                num62 = num63;

            if (num59 > 0)
                text2 = text2 + "[c/" + Colors.AlphaDarken(Colors.CoinPlatinum).Hex3() + ":" + num59 + " " + Lang.inter[15].Value + "] ";

            if (num60 > 0)
                text2 = text2 + "[c/" + Colors.AlphaDarken(Colors.CoinGold).Hex3() + ":" + num60 + " " + Lang.inter[16].Value + "] ";

            if (num61 > 0)
                text2 = text2 + "[c/" + Colors.AlphaDarken(Colors.CoinSilver).Hex3() + ":" + num61 + " " + Lang.inter[17].Value + "] ";

            if (num62 > 0)
                text2 = text2 + "[c/" + Colors.AlphaDarken(Colors.CoinCopper).Hex3() + ":" + num62 + " " + Lang.inter[18].Value + "] ";
            
            if (cost == 0)
                text2 = $"[c/{Colors.AlphaDarken(Colors.RarityTrash).Hex3()}:未知]";
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, text2, new Vector2(pos.X , pos.Y),Color.White, 0f, Vector2.Zero, Vector2.One);

        }

        public void BrewPotion()
        {
            if (potionSetting.slider.value==0)
                return;
            Craft?.Invoke(AsPotion(PreviewPotion),currentItem);
            Potion = new();
            Potion.SetDefaults(ModContent.ItemType<BasePotion>());
            SetModItem.Invoke(Potion, [PreviewPotion.ModItem]);
            Potion.stack = potionSetting.slider.value;
            Main.LocalPlayer.GetItem(Main.LocalPlayer.whoAmI,Potion, GetItemSettings.ItemCreatedFromItemUsage);
            Main.LocalPlayer.BuyItem(1000 * potionSetting.slider.value);
            ClearAll();
        }

        public void Refresh()
        {
            if(PreviewPotion is null)
            {
                PreviewPotion = new Item();
                PreviewPotion.SetDefaults(ModContent.ItemType<BasePotion>());
                CreatPotion = PreviewPotion.ModItem as BasePotion;
            }
            SetModItem.Invoke(PreviewPotion, [CreatPotion]);
            potionComponent.ComponentUpdate();
            PotionSynopsis.SynopsisUpdate();
        }

        public void ClearAll()
        {
            PreviewPotion = new();
            PreviewPotion.SetDefaults(ModContent.ItemType<BasePotion>());
            CreatPotion = PreviewPotion.ModItem as BasePotion;
            PotionMaterial = null;
            Craft = null;
            potionCrucible.Putity.Active = false;
            potionCrucible.MashUp.Active = false;
            Refresh();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            PotionSynopsis.Update(gameTime);
            potionSetting.Update(gameTime);
            potionComponent.Update(gameTime);
            potionCrucible.Update(gameTime);
            //potionBaseSelect.Update(gameTime);
        }
        
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
        }

    }

    public class PotionCrucible : PotionElement<BrewPotionState>
    {
        private UIElement Crucible;

        public Button MashUp;

        public Button Putity;

        private BrewPotionState BrewPotionState;
        public PotionCrucible(BrewPotionState brewPotionState)
        {
            BrewPotionState = brewPotionState;
            PotionCraftState = brewPotionState;
            Height.Set(400, 0);
            Width.Set(400, 0);
            Crucible = new UIElement();
            Crucible.Width.Set(384, 0);
            Crucible.Height.Set(234, 0);
            Crucible.Top.Set(250, 0);
            Crucible.HAlign = .5f;
            Append(Crucible);

            MashUp = new(Assets.UI.HelpIcon, Color.White, brewPotionState)
            {
                Name = "MashUp",
                Active = false
            };
            MashUp.Width.Set(32, 0);
            MashUp.Height.Set(32, 0);
            MashUp.Top.Set(160, 0);
            MashUp.Left.Set(360, 0);
            MashUp.OnClike = () =>
            {
                brewPotionState.MashUp(brewPotionState.CreatPotion, brewPotionState.currentItem);
                brewPotionState.Craft = null;
                brewPotionState.PotionMaterial = null;
                Putity.Active = false;
                MashUp.Active = false;
                brewPotionState.Refresh();
            };
            MashUp.TransitionAnimation = () =>
            {
                MashUp.A = MathHelper.Lerp(MashUp.A, MashUp.Active ? 1 : .7f, .05f);
            };
            MashUp.HoverTexture = Assets.UI.HelpIconActive;
            Append(MashUp);

            Putity = new(Assets.UI.HelpIcon, Color.White, brewPotionState)
            {
                Name = "Putity",
                Active = false
            };
            Putity.Width.Set(32, 0);
            Putity.Height.Set(32, 0);
            Putity.Top.Set(200, 0);
            Putity.Left.Set(360, 0);
            Putity.OnClike = () =>
            {
                brewPotionState.MashUp(brewPotionState.CreatPotion, brewPotionState.currentItem);
                brewPotionState.Craft = null;
                brewPotionState.PotionMaterial = null;
                Putity.Active = false;
                MashUp.Active = false;
                brewPotionState.Refresh();
            };
            Putity.TransitionAnimation = () =>
            {
                Putity.A = MathHelper.Lerp(Putity.A, Putity.Active ? 1 : .7f, .05f);
            };
            Putity.HoverTexture = Assets.UI.HelpIconActive;
            Append(Putity);

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            MashUp.Update(gameTime);
            Putity.Update(gameTime);
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            if (!PotionCraftState.Active) return;
            if (Main.mouseItem.IsAir)
                return;

            if (!PotionList.ContainsKey(Main.mouseItem.Name))
                return;

            BrewPotionState.AddPotion(Main.mouseItem);
            Main.LocalPlayer.HeldItem.TurnToAir();
            Main.mouseItem.TurnToAir();
            
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Draw(Assets.UI.Crucible, Crucible.GetDimensions().ToRectangle(), Color.White);

        }

    }

}
