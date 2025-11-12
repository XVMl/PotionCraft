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

namespace PotionCraft.Content.UI.CraftUI
{
    public class BrewPotionState : AutoUIState
    {
        public override bool Active() => ActiveState && CraftState == CraftUiState.BrewPotion;

        public override bool Isload() => true;

        public static readonly MethodInfo SetModItem = typeof(Item).GetProperty(nameof(Item.ModItem))!.GetSetMethod();
        
        public override string LayersFindIndex => "Vanilla: Mouse Text";

        private PotionSlot<BrewPotionState> PotionSlot;
        
        private Item currentItem;

        private BrewPotionButton BrewPotionButton;

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

        private bool changecraft;
        public override void OnInitialize()
        {
            PotionSynopsis = new PotionSynopsis(this);
            PotionSynopsis.Top.Set(300, 0);
            PotionSynopsis.Left.Set(500, 0);
            PotionSynopsis.SourcePotion = new Vector2(PotionSynopsis.Left.Pixels, PotionSynopsis.Top.Pixels);
            Append(PotionSynopsis);

            colorSelector = new(this);
            colorSelector.Top.Set(300, 0);
            colorSelector.Left.Set(100, 0);
            Append(colorSelector);

            potionSetting = new PotionSetting(this)
            {
                HAlign = .5f,
                VAlign = .7f
            };
            Append(potionSetting);

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
        }
        
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            
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
            Width.Set(342f, 0);
            Height.Set(388f, 0);
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
            var tex = UITexture("Ui3").Value;
            spriteBatch.Draw(tex, GetDimensions().Position() + new Vector2(0,-24),new Rectangle(0,0,342,24), Color.White);
            spriteBatch.Draw(tex, GetDimensions().Position(), Color.White);
            spriteBatch.Draw(tex, GetDimensions().Position() + new Vector2(0, +24), new Rectangle(0, 164, 342, 24), Color.White);
        }
    }

    public class PotionSetting : PotionElement<BrewPotionState>
    {
        private Button delete;

        private Button brew;

        private Slider slider;

        private Button autouse;

        private Button packing;

        public PotionSetting(BrewPotionState brewPotionState)
        {
            PotionCraftState = brewPotionState;
            Width.Set(384f, 0);
            Height.Set(212f, 0);
            delete = new Button(UITexture("Delete"), Color.White, "Delete");
            delete.Height.Set(34, 0);
            delete.Width.Set(96, 0);
            delete.Left.Set(50, 0);
            delete.Top.Set(170, 0);
            Append(delete);
            brew = new Button(UITexture("Brew"), Color.White, "Brew");
            brew.Height.Set(34, 0);
            brew.Width.Set(96, 0);
            brew.Left.Set(250, 0);
            brew.Top.Set(170, 0);
            Append(brew);
            slider = new Slider(brewPotionState);
            slider.Left.Set(90, 0);
            slider.Top.Set(122, 0);
            Append(slider);
            autouse = new Button(Assets.UI.Icon, Color.White, new Rectangle(40,0,18,18));
            autouse.Height.Set(18, 0);
            autouse.Width.Set(18, 0);
            autouse.Left.Set(122, 0);
            autouse.Top.Set(46, 0);
            autouse.Iconcolor = Deafult;
            autouse.Value = brewPotionState.CreatPotion.AutoUse;
            autouse.OnClike = () =>
            {
                autouse.Value = !autouse.Value;
                brewPotionState.CreatPotion.AutoUse = !autouse.Value;;
            };
            Append(autouse);

            packing = new Button(Assets.UI.Icon, Color.White, new Rectangle(20,0,18,18));
            packing.Height.Set(18, 0);
            packing.Width.Set(18, 0);
            packing.Left.Set(26, 0);
            packing.Top.Set(46, 0);
            packing.Iconcolor = Deafult;
            packing.OnClike = () =>
            {
                packing.Value = !packing.Value;
                brewPotionState.CreatPotion.IsPackage = !packing.Value;
                packing.Rectangle = packing.Value ? new Rectangle(0, 0, 18, 18) : new Rectangle(20, 0, 18, 18); ;
            };
            Append(packing);
            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            slider.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.UI.UI4, GetDimensions().ToRectangle(), Color.White);
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
