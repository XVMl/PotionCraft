using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.Items;
using PotionCraft.Content.System;
using System;
using System.Reflection;
using Terraria.ModLoader;
using static PotionCraft.Assets;
using Terraria.UI;
using Terraria;
using Microsoft.Xna.Framework;
using static PotionCraft.Content.System.AutoLoaderSystem.LoaderPotionOrMaterial;
using static PotionCraft.Content.System.LanguageHelper;
using PotionCraft.Content.UI.PotionTooltip;
using Steamworks;
using Terraria.ID;
using static System.Net.Mime.MediaTypeNames;
using Terraria.GameContent;
using Terraria.UI.Chat;
using Terraria.Map;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;

namespace PotionCraft.Content.UI.CraftUI
{
    public class BrewPotionState : AutoUIState
    {
        public override bool Active() => ActiveState && CraftState == CraftUiState.BrewPotion;

        public override bool Isload() => true;

        public static readonly MethodInfo SetModItem = typeof(Item).GetProperty(nameof(Item.ModItem))!.GetSetMethod();
        
        public override string LayersFindIndex => "Vanilla: Mouse Text";
        
        private Item currentItem;

        public int PotionStack = 20;

        public BasePotion CreatPotion = ModContent.GetInstance<BasePotion>();
        
        public Item PreviewPotion;
        
        public BaseCustomMaterials CustomMaterials =  ModContent.GetInstance<BaseCustomMaterials>();
        
        private bool ConflitCraft;
        
        private Action<BasePotion,Item> Craft;
        
        private PotionSynopsis PotionSynopsis;

        private PotionComponent potionComponent;

        private PotionSetting potionSetting;

        public ColorSelector colorSelector;

        public override void OnInitialize()
        {
            potionSetting = new PotionSetting(this)
            {
                HAlign = .5f,
                VAlign = .7f
            };
            Append(potionSetting);

            PotionSynopsis = new PotionSynopsis(this);
            PotionSynopsis.Top.Set(300, 0);
            PotionSynopsis.Left.Set(370, 0);
            //PotionSynopsis.SourcePotion = new Vector2(PotionSynopsis.Left.Pixels, PotionSynopsis.Top.Pixels);
            Append(PotionSynopsis);

            colorSelector = new(this);
            colorSelector.Top.Set(300, 0);
            colorSelector.Left.Set(20, 0);
            Append(colorSelector);

            potionComponent = new PotionComponent(this);
            potionComponent.Top.Set(228, 0);
            potionComponent.Left.Set(1150, 0);
            Append(potionComponent);

        }

        #region 操作具体方法

        public static bool QuicklyCheckPotion(Item item1,BasePotion item2)
        {
            if (item1.ModItem is not BasePotion)
                return false;
            return AsPotion(item1)._Name == item2._Name;
        }
        
        private void AddPotion(Item item)
        {
            if (PotionList.ContainsKey(item) && item.ModItem is not BasePotion)
                return;

            Craft.Invoke(CreatPotion,currentItem);
            ConflitCraft =  false;
            currentItem = item;
            if (item.type == ModContent.ItemType<MagicPanacea>())
            {
                Craft = Putify;
                return; 
            } 
            if (!QuicklyCheckPotion(item, CreatPotion) )
            {
                Craft =MashUp;
                return;
            }
            ConflitCraft = true;
            Refresh();
        }
        
        private void Putify(BasePotion potion,Item item)
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

        private void MashUp(BasePotion potion,Item item)
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
                $"{Lang.GetBuffName(item.buffType).Replace(" ", "")} + ";    
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
                potion._Name = $"{Lang.GetBuffName(item.buffType).Replace(" ","")} ";
        }

        #endregion
        
        private void ChangeCraft()
        {
            if (ConflitCraft)
                Craft = Equals(Craft, (Action<BasePotion,Item>)Putify) ? MashUp : Putify;
            Refresh();
        }
        
        public static void DrawCost(SpriteBatch spriteBatch,int cost,Vector2 pos)
        {
            string text2 = "";
            int num59 = 0;
            int num60 = 0;
            int num61 = 0;
            int num62 = 0;
            int num63 = cost;
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
            
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, text2, new Vector2(pos.X , pos.Y),Color.White, 0f, Vector2.Zero, Vector2.One);

        }

        private void BrewPotion(Item potion)
        {
            Craft.Invoke(CreatPotion,currentItem);
            Potion.SetDefaults(ModContent.ItemType<BasePotion>());
            SetModItem.Invoke(Potion, [CreatPotion]);
            Potion.stack = PotionStack;
            Main.LocalPlayer.GetItem(Main.LocalPlayer.whoAmI, PreviewPotion, GetItemSettings.ItemCreatedFromItemUsage);
            Main.LocalPlayer.BuyItem(50 * PotionStack);
            ClearAll();
        }

        private void Refresh()
        {
            PreviewPotion = new Item();
            PreviewPotion.SetDefaults(ModContent.ItemType<BasePotion>());
            var preview = TransExp<BasePotion, BasePotion>.Trans(CreatPotion);
            Craft.Invoke(preview, currentItem);
            potionComponent.ComponentUpdate();
            PotionSynopsis.SynopsisUpdate();
            SetModItem.Invoke(PreviewPotion.ModItem, [preview]);
        }
        
        private void ClearAll()
        {
            CreatPotion = ModContent.GetInstance<BasePotion>();
            Craft = null;
            Potion = new Item();
            Potion.SetDefaults(ModContent.ItemType<BasePotion>());
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            PotionSynopsis.Update(gameTime);
            potionSetting.Update(gameTime);
            colorSelector.Update(gameTime);

        }
        
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            //ItemSlot.DrawSavings(spriteBatch, 600, 400);
        }

    }

    public class PotionComponent : PotionElement<BrewPotionState>
    {
        private PotionIngredients _potionIngredients;

        private BrewPotionState _brewPotionState;
        
        public PotionComponent(BrewPotionState brewPotionState)
        {
            PotionCraftState = brewPotionState;
            _brewPotionState = brewPotionState;
            Width.Set(346f, 0);
            Height.Set(486f, 0);
            _potionIngredients = new PotionIngredients(brewPotionState);
            _potionIngredients.Top.Set(30, 0);
            _potionIngredients.Left.Set(0, 0);
            _potionIngredients.Width.Set(350, 0);
            _potionIngredients.Height.Set(500, 0);
            Append(_potionIngredients);
            
        }

        public void ComponentUpdate()
        {
            var potion = _brewPotionState.PreviewPotion.ModItem as BasePotion;
            _potionIngredients.UIgrid.Clear();
            _potionIngredients.SetPotionCraftState(potion);
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Draw(Assets.UI.UI2, GetDimensions().Position(), Color.White);
        }
    }

    public class PotionSetting : PotionElement<BrewPotionState>
    {
        private UIElement BG;

        private Button delete;

        public Button brew;

        private Slider slider;

        private Button autouse;

        private Button potionlock;

        private Button packing;

        private Button Hajimi;

        public PotionSetting(BrewPotionState brewPotionState)
        {
            Width.Set(384f, 0);
            Height.Set(300f, 0);
            
            BG = new();
            BG.Width.Set(384, 0);
            BG.Height.Set(224, 0);
            Append(BG);

            PotionCraftState = brewPotionState;
            delete = new Button(UITexture("Delete"), Color.White, "Delete");
            delete.Height.Set(34, 0);
            delete.Width.Set(96, 0);
            delete.Left.Set(50, 0);
            delete.Top.Set(170, 0);
            BG.Append(delete);

            brew = new Button(UITexture("Brew"), Color.White, "Brew");
            brew.Height.Set(34, 0);
            brew.Width.Set(96, 0);
            brew.Left.Set(250, 0);
            brew.Top.Set(170, 0);
            BG.Append(brew);

            slider = new Slider(brewPotionState,"数量");
            slider.Left.Set(90, 0);
            slider.Top.Set(112, 0);
            slider.text.TextColor = Deafult;
            BG.Append(slider);

            autouse = new Button(Assets.UI.Icon, Color.White, new Rectangle(40,0,18,18));
            autouse.Height.Set(18, 0);
            autouse.Width.Set(18, 0);
            autouse.Left.Set(116, 0);
            autouse.Top.Set(46, 0);
            autouse.Iconcolor = Deafult;
            autouse.Value = brewPotionState.CreatPotion.AutoUse;
            autouse.OnClike = () =>
            {
                autouse.Value = !autouse.Value;
                brewPotionState.CreatPotion.AutoUse = !autouse.Value;;
            };
            BG.Append(autouse);

            potionlock = new Button(Assets.UI.Icon, Color.White, new Rectangle(62, 0, 18, 18));
            potionlock.Height.Set(18, 0);
            potionlock.Width.Set(18, 0);
            potionlock.Left.Set(40, 0);
            potionlock.Top.Set(46, 0);
            potionlock.Iconcolor = Deafult;
            potionlock.OnClike = () =>
            {
                potionlock.Value = !potionlock.Value;
                brewPotionState.CreatPotion.IsPackage = !potionlock.Value;
                potionlock.Rectangle = potionlock.Value ? new Rectangle(62, 0, 18, 18) : new Rectangle(80, 0, 18, 18); ;
            };
            BG.Append(potionlock);

            packing = new Button(Assets.UI.Icon, Color.White, new Rectangle(20, 0, 18, 18));
            packing.Height.Set(18, 0);
            packing.Width.Set(18, 0);
            packing.Left.Set(76, 0);
            packing.Top.Set(46, 0);
            packing.Iconcolor = Deafult;
            packing.OnClike = () =>
            {
                packing.Value = !packing.Value;
                brewPotionState.CreatPotion.IsPackage = !packing.Value;
                packing.Rectangle = packing.Value ? new Rectangle(0, 0, 18, 18) : new Rectangle(20, 0, 18, 18); ;
            };
            BG.Append(packing);

            Hajimi = new(Assets.UI.HajimiIcon, Color.White);
            Hajimi.Width.Set(40, 0);
            Hajimi.Height.Set(34, 0);
            Hajimi.Top.Set(240, 0);
            Hajimi.Left.Set(300, 0);
            Hajimi.HoverTexture = Assets.UI.HajimiIconHover;
            Append(Hajimi);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            slider.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.UI.UI4, BG.GetDimensions().ToRectangle(), Color.White);
            Utils.DrawBorderString(spriteBatch, "花费:", GetDimensions().Position() + new Vector2(40, 80), Deafult, 1f);
            BrewPotionState.DrawCost(spriteBatch, slider.value*1000, GetDimensions().Position()+new Vector2(90,80));
            ItemSlot.DrawSavings(spriteBatch, GetDimensions().X, GetDimensions().Y+200);
            base.Draw(spriteBatch);
        }

    }

    public class PotionCrucible : PotionElement<BrewPotionState>
    {
                
    }

    public class BrewPotionButton : PotionElement<BrewPotionState>
    {
        public BrewPotionButton(BrewPotionState brewPotionState)
        {
            PotionCraftState = brewPotionState;
            Width.Set(100f, 0);
            Height.Set(32f, 0);
        }

        // private void BrewPotion(Item potion)
        // {
        //     if (PotionCraftState.Crafts.Count != 0)
        //         foreach (var craft in PotionCraftState.Crafts) craft.Invoke();
        //     
        //     SetModItem.Invoke(PotionCraftState.Potion, [PotionCraftState.CreatPotion]);
        //     Main.LocalPlayer.BuyItem(50 * PotionCraftState.PotionCount);
        // }

        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
            if (PotionCraftState.Potion.IsAir) return;
            // BrewPotion((PotionCraftState.Potion));
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            var rectangle = GetDimensions().ToRectangle();
            spriteBatch.Draw(Assets.UI.Button, rectangle, Color.White);
            ItemSlot.DrawSavings(spriteBatch,rectangle.X, rectangle.Y);
        }

    }
}
